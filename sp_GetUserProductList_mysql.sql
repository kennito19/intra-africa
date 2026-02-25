DROP PROCEDURE IF EXISTS `sp_GetUserProductList`;

DELIMITER //

CREATE PROCEDURE `sp_GetUserProductList`(
    IN p_categoryid INT,
    IN p_sellerids LONGTEXT,
    IN p_brandids LONGTEXT,
    IN p_searchtext LONGTEXT,
    IN p_sizeids LONGTEXT,
    IN p_colorids LONGTEXT,
    IN p_productcollectionid VARCHAR(10),
    IN p_guids LONGTEXT,
    IN p_minprice LONGTEXT,
    IN p_maxprice LONGTEXT,
    IN p_mindiscount LONGTEXT,
    IN p_AvailableProductsOnly TINYINT(1),
    IN p_PriceSort INT,
    IN p_SpecTypeIds LONGTEXT,
    IN p_pageIndex INT,
    IN p_pageSize INT,
    IN p_date VARCHAR(100),
    INOUT p_output INT,
    INOUT p_message VARCHAR(50)
)
BEGIN
    DECLARE v_recordCount INT DEFAULT 0;
    DECLARE v_pageCount   INT DEFAULT 0;
    DECLARE v_startRow    INT DEFAULT ((p_pageIndex - 1) * p_pageSize) + 1;
    DECLARE v_endRow      INT DEFAULT p_pageIndex * p_pageSize;

    -- Clean up any stale temp tables from prior calls
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_raw;
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_best;
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_filt;
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_page;
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_final;

    -- -----------------------------------------------------------------------
    -- Step 1: Gather all matching child products (best price selected via RN=1)
    -- -----------------------------------------------------------------------
    CREATE TEMPORARY TABLE tmp_upl_raw AS
    SELECT
        p.Id                                                        AS ProductID,
        p.ParentId,
        p.CategoryId,
        p.AssiCategoryId,
        p.ProductName,
        p.CustomeProductName,
        p.CompanySKUCode,
        p.Guid,
        p.IsMasterProduct,
        CAST(p.CreatedAt  AS CHAR)                                  AS CreatedAt,
        CAST(p.ModifiedAt AS CHAR)                                  AS ModifiedAt,
        p.Highlights,
        c.Name                                                      AS CategoryName,
        c.PathIds                                                   AS CategoryPathIds,
        c.PathNames                                                 AS CategoryPathNames,
        sp.Id                                                       AS SellerProductId,
        sp.SellerID                                                 AS SellerId,
        sp.BrandID                                                  AS BrandId,
        sp.Status,
        sp.Live,
        sp.ExtraDetails,
        pp.MRP,
        pp.SellingPrice,
        pp.Discount,
        pp.Quantity,
        ROW_NUMBER() OVER (PARTITION BY p.Id ORDER BY COALESCE(pp.SellingPrice, 999999999) ASC) AS rn
    FROM ProductMaster p
    INNER JOIN SellerProductMaster sp ON sp.ProductID = p.Id
    LEFT  JOIN ProductPriceMaster pp  ON pp.SellerProductID = sp.Id AND pp.IsDeleted = 0
    LEFT  JOIN categoryLibrary c      ON c.Id = p.CategoryId
    WHERE
        p.ParentId IS NOT NULL
        AND p.IsDeleted = 0
        AND sp.Status    = 'Active'
        AND sp.Live      = 1
        AND sp.IsDeleted = 0
        -- Seller filter
        AND (p_sellerids IS NULL OR p_sellerids = '' OR
             INSTR(CONVERT(CONCAT(',', p_sellerids, ',') USING latin1), CONVERT(CONCAT(',', sp.SellerID, ',') USING latin1)) > 0)
        -- Brand filter
        AND (p_brandids IS NULL OR p_brandids = '' OR
             INSTR(CONVERT(CONCAT(',', p_brandids, ',') USING latin1), CONVERT(CONCAT(',', CAST(sp.BrandID AS CHAR), ',') USING latin1)) > 0)
        -- Category filter (includes sub-categories via PathIds hierarchy)
        AND (
            p_categoryid IS NULL OR p_categoryid = 0
            OR c.PathIds  = CAST(p_categoryid AS CHAR)
            OR c.PathIds  LIKE CONCAT(CAST(p_categoryid AS CHAR), '>%')
            OR c.PathIds  LIKE CONCAT('%>', CAST(p_categoryid AS CHAR), '>%')
            OR c.PathIds  LIKE CONCAT('%>', CAST(p_categoryid AS CHAR))
        )
        -- Size filter (on price master)
        AND (p_sizeids IS NULL OR p_sizeids = '' OR
             INSTR(CONVERT(CONCAT(',', p_sizeids, ',') USING latin1), CONVERT(CONCAT(',', CAST(COALESCE(pp.SizeID, '') AS CHAR), ',') USING latin1)) > 0)
        -- Color filter
        AND (p_colorids IS NULL OR p_colorids = '' OR
             p.Id IN (
                 SELECT DISTINCT pcm2.ProductID
                 FROM ProductColorMapping pcm2
                 WHERE INSTR(CONVERT(CONCAT(',', p_colorids, ',') USING latin1), CONVERT(CONCAT(',', CAST(pcm2.ColorID AS CHAR), ',') USING latin1)) > 0
             ))
        -- Product collection filter
        AND (p_productcollectionid IS NULL OR p_productcollectionid = '' OR
             p.Id IN (
                 SELECT DISTINCT mcm.ProductId
                 FROM ManageCollectionMapping mcm
                 INNER JOIN ManageCollection mc ON mc.Id = mcm.CollectionId
                 WHERE mc.Status = 'Active'
                   AND mcm.IsDeleted = 0
                   AND mcm.CollectionId = CAST(p_productcollectionid AS UNSIGNED)
             ))
        -- GUID filter
        AND (p_guids IS NULL OR p_guids = '' OR
             INSTR(CONVERT(CONCAT(',', p_guids, ',') USING latin1), CONVERT(CONCAT(',', p.Guid, ',') USING latin1)) > 0)
        -- Search text filter
        AND (
            p_searchtext IS NULL OR p_searchtext = ''
            OR INSTR(CONVERT(p.ProductName        USING latin1), CONVERT(p_searchtext USING latin1)) > 0
            OR INSTR(CONVERT(p.CustomeProductName USING latin1), CONVERT(p_searchtext USING latin1)) > 0
            OR INSTR(CONVERT(p.CompanySKUCode     USING latin1), CONVERT(p_searchtext USING latin1)) > 0
            OR INSTR(CONVERT(c.Name               USING latin1), CONVERT(p_searchtext USING latin1)) > 0
            OR INSTR(CONVERT(sp.SKUCode           USING latin1), CONVERT(p_searchtext USING latin1)) > 0
            OR INSTR(CONVERT(p.Guid               USING latin1), CONVERT(p_searchtext USING latin1)) > 0
        );

    -- -----------------------------------------------------------------------
    -- Step 2: Keep only best-price row per product
    -- -----------------------------------------------------------------------
    CREATE TEMPORARY TABLE tmp_upl_best AS
    SELECT *
    FROM tmp_upl_raw
    WHERE rn = 1;

    -- -----------------------------------------------------------------------
    -- Step 3: Apply post-dedup filters (price, discount, availability)
    -- -----------------------------------------------------------------------
    CREATE TEMPORARY TABLE tmp_upl_filt AS
    SELECT ProductID, SellerProductId, SellingPrice, MRP, Discount, Quantity
    FROM tmp_upl_best
    WHERE
        Status = 'Active'
        AND Live = 1
        AND (p_minprice    IS NULL OR p_minprice    = '' OR SellingPrice >= CAST(p_minprice    AS DECIMAL(18,2)))
        AND (p_maxprice    IS NULL OR p_maxprice    = '' OR SellingPrice <= CAST(p_maxprice    AS DECIMAL(18,2)))
        AND (p_mindiscount IS NULL OR p_mindiscount = '' OR Discount     >= CAST(p_mindiscount AS DECIMAL(18,2)))
        AND (p_AvailableProductsOnly IS NULL OR p_AvailableProductsOnly = 0 OR COALESCE(Quantity, 0) > 0);

    SELECT COUNT(*) INTO v_recordCount FROM tmp_upl_filt;

    IF p_pageSize IS NULL OR p_pageSize = 0 THEN
        SET v_pageCount = 0;
    ELSE
        SET v_pageCount = CEILING(v_recordCount / p_pageSize);
    END IF;

    -- -----------------------------------------------------------------------
    -- Step 4: Paginate - assign row numbers over ordered filtered set
    -- -----------------------------------------------------------------------
    CREATE TEMPORARY TABLE tmp_upl_page AS
    SELECT
        f.ProductID,
        ROW_NUMBER() OVER (
            ORDER BY
                CASE WHEN p_PriceSort = 1 THEN f.SellingPrice END ASC,
                CASE WHEN p_PriceSort = 2 THEN f.SellingPrice END DESC,
                CASE WHEN p_PriceSort = 3 THEN f.Discount     END DESC,
                f.ProductID DESC
        ) AS rn
    FROM tmp_upl_filt f;

    -- -----------------------------------------------------------------------
    -- Step 5: Build final result temp table with both 'p' and 'f' flag rows
    -- -----------------------------------------------------------------------
    CREATE TEMPORARY TABLE tmp_upl_final (
        RowNumber       INT,
        flag            CHAR(1),
        RecordCount     INT,
        PageCount       INT,
        Id              INT,
        Guid            VARCHAR(36),
        IsMasterProduct TINYINT(1),
        ParentId        INT,
        CategoryId      INT,
        AssiCategoryId  INT,
        ProductName     VARCHAR(500),
        CustomeProductName VARCHAR(500),
        CompanySKUCode  VARCHAR(250),
        Image1          LONGTEXT,
        MRP             DECIMAL(18,2),
        SellingPrice    DECIMAL(18,2),
        Discount        DECIMAL(18,2),
        Quantity        INT,
        CreatedAt       VARCHAR(100),
        ModifiedAt      VARCHAR(100),
        Highlights      LONGTEXT,
        CategoryName    VARCHAR(250),
        CategoryPathIds VARCHAR(500),
        CategoryPathNames LONGTEXT,
        SellerProductId INT,
        SellerId        VARCHAR(500),
        BrandId         INT,
        BrandName       VARCHAR(250),
        TotalQty        INT,
        Status          VARCHAR(50),
        Live            TINYINT(1),
        ExtraDetails    LONGTEXT,
        TotalVariant    INT,
        F_CategoryId    INT,
        F_CategoryName  VARCHAR(200),
        F_ProductCount  INT,
        F_BrandId       INT,
        F_BrandName     VARCHAR(200),
        F_SizeID        INT,
        F_Size          VARCHAR(500),
        F_Quantity      INT,
        F_ColorID       INT,
        F_ColorName     VARCHAR(500),
        F_ColorCode     VARCHAR(500),
        MinSellingPrice DECIMAL(18,2),
        MaxSellingPrice DECIMAL(18,2),
        FilterTypeId    INT,
        FilterTypeName  VARCHAR(500),
        FilterValueId   INT,
        FilterValueName VARCHAR(500)
    );

    -- -----------------------------------------------------------------------
    -- Step 6: Insert product rows (flag = 'p')
    -- -----------------------------------------------------------------------
    INSERT INTO tmp_upl_final (
        RowNumber, flag, RecordCount, PageCount,
        Id, Guid, IsMasterProduct, ParentId, CategoryId, AssiCategoryId,
        ProductName, CustomeProductName, CompanySKUCode,
        Image1, MRP, SellingPrice, Discount, Quantity,
        CreatedAt, ModifiedAt, Highlights,
        CategoryName, CategoryPathIds, CategoryPathNames,
        SellerProductId, SellerId, BrandId, BrandName, TotalQty,
        Status, Live, ExtraDetails, TotalVariant
    )
    SELECT
        pg.rn,
        'p',
        v_recordCount,
        v_pageCount,
        b.ProductID,
        b.Guid,
        b.IsMasterProduct,
        b.ParentId,
        b.CategoryId,
        COALESCE(b.AssiCategoryId, 0),
        b.ProductName,
        b.CustomeProductName,
        b.CompanySKUCode,
        (SELECT i.Url
         FROM ProductImages i
         WHERE i.ProductID = b.ProductID AND i.Type = 'Image'
         ORDER BY i.Sequence
         LIMIT 1),
        b.MRP,
        b.SellingPrice,
        COALESCE(b.Discount, 0),
        COALESCE(b.Quantity, 0),
        b.CreatedAt,
        b.ModifiedAt,
        b.Highlights,
        b.CategoryName,
        b.CategoryPathIds,
        b.CategoryPathNames,
        b.SellerProductId,
        b.SellerId,
        b.BrandId,
        JSON_UNQUOTE(JSON_EXTRACT(b.ExtraDetails, '$.BrandDetails.Name')),
        COALESCE(b.Quantity, 0),
        b.Status,
        b.Live,
        b.ExtraDetails,
        (SELECT COUNT(DISTINCT pm2.Id)
         FROM ProductMaster pm2
         INNER JOIN SellerProductMaster spm2 ON pm2.Id = spm2.ProductID
         WHERE pm2.ParentId = b.ProductID
           AND spm2.Status != 'Archived'
           AND spm2.IsExistingProduct = 0)
    FROM tmp_upl_best b
    INNER JOIN tmp_upl_page pg ON pg.ProductID = b.ProductID
    WHERE pg.rn BETWEEN v_startRow AND v_endRow
    ORDER BY pg.rn;

    -- -----------------------------------------------------------------------
    -- Step 7: Insert filter rows (flag = 'f')
    -- -----------------------------------------------------------------------

    -- 7a. Min / Max selling price
    INSERT INTO tmp_upl_final (flag, MinSellingPrice, MaxSellingPrice)
    SELECT 'f', MIN(SellingPrice), MAX(SellingPrice)
    FROM tmp_upl_filt;

    -- 7b. Distinct discount values (for discount filter slider)
    INSERT INTO tmp_upl_final (flag, Discount)
    SELECT DISTINCT 'f', Discount
    FROM tmp_upl_filt
    WHERE Discount IS NOT NULL AND Discount > 0;

    -- 7c. Category filters
    INSERT INTO tmp_upl_final (flag, F_CategoryId, F_CategoryName, F_ProductCount)
    SELECT 'f', c.Id, c.Name, COUNT(DISTINCT b.ProductID)
    FROM tmp_upl_best b
    INNER JOIN tmp_upl_filt f ON f.ProductID = b.ProductID
    INNER JOIN categoryLibrary c ON c.Id = b.CategoryId
    GROUP BY c.Id, c.Name;

    -- 7d. Brand filters
    INSERT INTO tmp_upl_final (flag, F_BrandId, F_BrandName)
    SELECT DISTINCT 'f', b.BrandId,
        JSON_UNQUOTE(JSON_EXTRACT(b.ExtraDetails, '$.BrandDetails.Name'))
    FROM tmp_upl_best b
    INNER JOIN tmp_upl_filt f ON f.ProductID = b.ProductID
    WHERE b.BrandId IS NOT NULL;

    -- 7e. Size filters
    INSERT INTO tmp_upl_final (flag, F_SizeID, F_Size, F_Quantity)
    SELECT 'f', sl.Id, sl.TypeName, SUM(pp2.Quantity)
    FROM ProductPriceMaster pp2
    INNER JOIN sizeLibrary sl ON sl.Id = pp2.SizeID
    INNER JOIN tmp_upl_filt f ON f.SellerProductId = pp2.SellerProductID
    WHERE pp2.IsDeleted = 0 AND pp2.SizeID IS NOT NULL
    GROUP BY sl.Id, sl.TypeName;

    -- 7f. Color filters
    INSERT INTO tmp_upl_final (flag, F_ColorID, F_ColorName, F_ColorCode)
    SELECT DISTINCT 'f', cl.Id, cl.Name, cl.Code
    FROM ProductColorMapping pcm
    INNER JOIN ColorLibrary cl ON cl.Id = pcm.ColorID
    INNER JOIN tmp_upl_filt f  ON f.ProductID = pcm.ProductID;

    -- 7g. Specification/attribute filters
    INSERT INTO tmp_upl_final (flag, FilterTypeId, FilterTypeName, FilterValueId, FilterValueName)
    SELECT DISTINCT 'f',
        stvm.SpecTypeId,
        sl2.Name,
        stvm.SpecValueId,
        stvm.Value
    FROM ProductSpecificationMapping stvm
    INNER JOIN specificationLibrary sl2 ON sl2.ID = stvm.SpecTypeId
    INNER JOIN AssignSpecValuesToCategory asvc ON asvc.SpecTypeID = stvm.SpecTypeId
    INNER JOIN AssignSpecificationToCategory aspc ON aspc.Id = asvc.AssignSpecID
    INNER JOIN tmp_upl_best b ON b.ProductID = stvm.ProductID
    INNER JOIN tmp_upl_filt f ON f.ProductID = stvm.ProductID
    WHERE b.CategoryId = aspc.CategoryID
      AND asvc.IsAllowSpecInFilter = 1;

    -- -----------------------------------------------------------------------
    -- Final SELECT – single result set for the C# reader
    -- -----------------------------------------------------------------------
    SELECT
        RowNumber, flag, RecordCount, PageCount,
        Id, Guid, IsMasterProduct, ParentId, CategoryId, AssiCategoryId,
        ProductName, CustomeProductName, CompanySKUCode,
        Image1, MRP, SellingPrice, Discount, Quantity,
        CreatedAt, ModifiedAt, Highlights,
        CategoryName, CategoryPathIds, CategoryPathNames,
        SellerProductId, SellerId, BrandId, BrandName, TotalQty,
        Status, Live, ExtraDetails, TotalVariant,
        F_CategoryId, F_CategoryName, F_ProductCount,
        F_BrandId, F_BrandName,
        F_SizeID, F_Size, F_Quantity,
        F_ColorID, F_ColorName, F_ColorCode,
        MinSellingPrice, MaxSellingPrice,
        FilterTypeId, FilterTypeName, FilterValueId, FilterValueName
    FROM tmp_upl_final
    ORDER BY flag ASC, RowNumber ASC;

    -- Cleanup
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_raw;
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_best;
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_filt;
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_page;
    DROP TEMPORARY TABLE IF EXISTS tmp_upl_final;

    SET p_output  = 0;
    SET p_message = 'Success';
END //

DELIMITER ;

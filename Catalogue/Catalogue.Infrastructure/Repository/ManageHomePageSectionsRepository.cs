using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Catalogue.Infrastructure.Repository
{
    public class ManageHomePageSectionsRepository : IManageHomePageSectionsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
        
        public ManageHomePageSectionsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ManageHomePageSectionsLibrary homePageSection)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@layoutId", homePageSection.LayoutId),
                    new SqlParameter("@layoutTypeId", homePageSection.LayoutTypeId),
                    new SqlParameter("@name", homePageSection.Name),
                    new SqlParameter("@sequence", homePageSection.Sequence),
                    new SqlParameter("@sectionColumns", homePageSection.SectionColumns),
                    new SqlParameter("@isTitleVisible", homePageSection.IsTitleVisible),
                    new SqlParameter("@title", homePageSection.Title),
                    new SqlParameter("@subTitle", homePageSection.SubTitle),
                    new SqlParameter("@titlePosition", homePageSection.TitlePosition),
                    new SqlParameter("@linkin", homePageSection.LinkIn),
                    new SqlParameter("@linktext", homePageSection.LinkText),
                    new SqlParameter("@link", homePageSection.Link),
                    new SqlParameter("@linkPosition", homePageSection.LinkPosition),
                    new SqlParameter("@status", homePageSection.Status),
                    new SqlParameter("@listtype", homePageSection.ListType),
                    new SqlParameter("@topproducts", homePageSection.TopProducts),
                    new SqlParameter("@totalRowsInSection", homePageSection.TotalRowsInSection),
                    new SqlParameter("@isCustomGrid", homePageSection.IsCustomGrid),
                    new SqlParameter("@numberOfImages", homePageSection.NumberOfImages),
                    new SqlParameter("@column1", homePageSection.Column1),
                    new SqlParameter("@column2", homePageSection.Column2),
                    new SqlParameter("@column3", homePageSection.Column3),
                    new SqlParameter("@column4", homePageSection.Column4),
                    new SqlParameter("@categoryid", homePageSection.CategoryId),
                    new SqlParameter("@backgroundColor", homePageSection.BackgroundColor),
                    new SqlParameter("@incontainer", homePageSection.InContainer),
                    new SqlParameter("@titleColor", homePageSection.TitleColor),
                    new SqlParameter("@textColor", homePageSection.TextColor),
                    new SqlParameter("@createdby", homePageSection.CreatedBy),
                new SqlParameter("@createdat", homePageSection.CreatedAt),
            };

                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter newid = new SqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHomePageSections, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(ManageHomePageSectionsLibrary homePageSection)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", homePageSection.Id),
                new SqlParameter("@layoutId", homePageSection.LayoutId),
                new SqlParameter("@layoutTypeId", homePageSection.LayoutTypeId),
                new SqlParameter("@name", homePageSection.Name),
                new SqlParameter("@sequence", homePageSection.Sequence),
                new SqlParameter("@sectionColumns", homePageSection.SectionColumns),
                new SqlParameter("@isTitleVisible", homePageSection.IsTitleVisible),
                new SqlParameter("@title", homePageSection.Title),
                new SqlParameter("@subTitle", homePageSection.SubTitle),
                new SqlParameter("@titlePosition", homePageSection.TitlePosition),
                new SqlParameter("@linkin", homePageSection.LinkIn),
                new SqlParameter("@linktext", homePageSection.LinkText),
                new SqlParameter("@link", homePageSection.Link),
                new SqlParameter("@linkPosition", homePageSection.LinkPosition),
                new SqlParameter("@status", homePageSection.Status),
                new SqlParameter("@listtype", homePageSection.ListType),
                new SqlParameter("@topproducts", homePageSection.TopProducts),
                new SqlParameter("@totalRowsInSection", homePageSection.TotalRowsInSection),
                new SqlParameter("@isCustomGrid", homePageSection.IsCustomGrid),
                new SqlParameter("@numberOfImages", homePageSection.NumberOfImages),
                new SqlParameter("@column1", homePageSection.Column1),
                new SqlParameter("@column2", homePageSection.Column2),
                new SqlParameter("@column3", homePageSection.Column3),
                new SqlParameter("@column4", homePageSection.Column4),
                new SqlParameter("@categoryid", homePageSection.CategoryId),
                new SqlParameter("@backgroundColor", homePageSection.BackgroundColor),
                new SqlParameter("@incontainer", homePageSection.InContainer),
                new SqlParameter("@titleColor", homePageSection.TitleColor),
                new SqlParameter("@textColor", homePageSection.TextColor),
                new SqlParameter("@modifiedby", homePageSection.ModifiedBy),
                new SqlParameter("@modifiedat", homePageSection.ModifiedAt),
            };

                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter newid = new SqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHomePageSections, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageHomePageSectionsLibrary homePageSection)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", homePageSection.Id),
            };

                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter newid = new SqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHomePageSections, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ManageHomePageSectionsLibrary>>> get(ManageHomePageSectionsLibrary homePageSection, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", homePageSection.Id),
                new SqlParameter("@layoutId", homePageSection.LayoutId),
                new SqlParameter("@layoutTypeId", homePageSection.LayoutTypeId),
                new SqlParameter("@name", homePageSection.Name),
                new SqlParameter("@layoutTypeName", homePageSection.LayoutTypeName),
                new SqlParameter("@layoutName", homePageSection.LayoutName),
                new SqlParameter("@status", homePageSection.Status),
                new SqlParameter("@searchtext", homePageSection.SearchText),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),
            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageHomePageSections, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ProductHomePageSectionLibrary>>> getProudctHomePageSection(ProductHomePageSectionLibrary proudctHomePageSection, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@categoryid", proudctHomePageSection.categoryId),
                new SqlParameter("@topproduct", proudctHomePageSection.topProduct),
                new SqlParameter("@productids", proudctHomePageSection.productIds),
                new SqlParameter("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageProductHomePageSections, ProuctLayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ManageHomePageSectionsLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<ManageHomePageSectionsLibrary> lstLayouts = new List<ManageHomePageSectionsLibrary>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new ManageHomePageSectionsLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    LayoutId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutId"))),
                    LayoutTypeId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutTypeId"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    Sequence = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Sequence")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Sequence"))),
                    SectionColumns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SectionColumns")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SectionColumns"))),
                    IsTitleVisible = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsTitleVisible")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsTitleVisible"))),
                    Title = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Title")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Title"))),
                    SubTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle"))),
                    TitlePosition = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitlePosition")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TitlePosition"))),
                    LinkIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkIn")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkIn"))),
                    LinkText = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkText")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkText"))),
                    Link = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Link")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Link"))),
                    LinkPosition = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkPosition")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkPosition"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    ListType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ListType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ListType"))),
                    TopProducts = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TopProducts")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TopProducts"))),
                    TotalRowsInSection = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalRowsInSection")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalRowsInSection"))),
                    IsCustomGrid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsCustomGrid")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCustomGrid"))),
                    NumberOfImages = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NumberOfImages")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("NumberOfImages"))),
                    Column1 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column1")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column1"))),
                    Column2 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column2")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column2"))),
                    Column3 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column3")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column3"))),
                    Column4 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column4")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column4"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    BackgroundColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BackgroundColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BackgroundColor"))),
                    InContainer = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InContainer")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("InContainer"))),
                    TitleColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleColor"))),
                    TextColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TextColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TextColor"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    LayoutName = Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutName"))),
                    LayoutTypeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeName"))),
                });
            }
            return lstLayouts;
        }

        private async Task<List<ProductHomePageSectionLibrary>> ProuctLayoutParserAsync(DbDataReader reader)
        {
            List<ProductHomePageSectionLibrary> lstLayouts = new List<ProductHomePageSectionLibrary>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new ProductHomePageSectionLibrary()
                {
                    id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Guid = Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    isMasterProduct = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("isMasterProduct"))),
                    parentId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("parentId"))),
                    categoryId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("categoryId"))),
                    assiCategoryId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("assiCategoryId"))),
                    productName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("productName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("productName"))),
                    customeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("customeProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("customeProductName"))),
                    companySkuCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("companySkuCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("companySkuCode"))),
                    image1 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("image1")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("image1"))),
                    mrp = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("mrp")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("mrp"))),
                    sellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("sellingPrice")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("sellingPrice"))),
                    discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("discount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("discount"))),
                    Quantity = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    createdAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    modifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    categoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("categoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("categoryName"))),
                    categoryPathIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("categoryPathIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("categoryPathIds"))),
                    categoryPathNames = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("categoryPathNames")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("categoryPathNames"))),
                    sellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("sellerProductId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("sellerProductId"))),
                    sellerId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("sellerId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("sellerId"))),
                    brandId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("brandId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("brandId"))),
                    brandName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("brandName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("brandName"))),
                    status = Convert.ToString(reader.GetValue(reader.GetOrdinal("status"))),
                    live = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("live"))),
                    extraDetails = Convert.ToString(reader.GetValue(reader.GetOrdinal("extraDetails"))),
                    totalVariant = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("totalVariant"))),
                });
            }
            return lstLayouts;
        }


    }
}

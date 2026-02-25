using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;

namespace API_Gateway.Common.products
{
    public class GetCommissionCharges
    {
        private readonly HttpContext _httpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ApiHelper helper;
        BaseResponse<CommissionChargesLibrary> baseResponse = new BaseResponse<CommissionChargesLibrary>();

        public GetCommissionCharges(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
        }

        public CommissionChargesLibrary GetCommission(int CategoryId, string? sellerId = null, int brandId = 0, string URL = null)
        {
            CommissionChargesLibrary chargesLibrary = new CommissionChargesLibrary();
            string url = string.Empty;
            
            if (!string.IsNullOrEmpty(sellerId))
            {
                url += "&SellerID=" + sellerId;
            }

            if (brandId != 0)
            {
                url += "&BrandID=" + brandId;
            }

            var response = helper.ApiCall(URL, EndPoints.CommissionCharges+ "/getCategoryWiseCommission" + "?CategoryID=" + CategoryId + url, "GET", null);
            if (response != null)
            {
                baseResponse = new BaseResponse<CommissionChargesLibrary>();
                baseResponse = baseResponse.JsonParseRecord(response);
                if (baseResponse.code == 200)
                {
                    chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                }
            }

                //if (!string.IsNullOrEmpty(sellerId) && brandId != 0 && CategoryId != 0)
                //{
                //    //chargesLibrary = CompulsaryCategoryBySellerandBrand(sellerId, brandId, URL, token, true);
                //    //if (chargesLibrary.ChargesIn == null)
                //    //{
                //    chargesLibrary = SpecificCategoryBySellerandBrand(CategoryId, sellerId, brandId, URL);
                //    //}
                //}
                //else if (!string.IsNullOrEmpty(sellerId))
                //{
                //    //chargesLibrary = CompulsaryCategoryBySeller(sellerId, URL, token, true);
                //    //if (chargesLibrary.ChargesIn == null && CategoryId != 0)
                //    //{
                //    chargesLibrary = SpecificCategoryBySeller(CategoryId, sellerId, URL);
                //    //}
                //}
                //else
                //{
                //    //chargesLibrary = CompulsaryCategory(URL, token, true);
                //    //if (chargesLibrary.ChargesIn == null && CategoryId != 0)
                //    //{
                //    chargesLibrary = SpecificCategory(CategoryId, URL);
                //    //}
                //}
                return chargesLibrary;
        }

        public CategoryLibrary getCategories(int categoryId, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?Id=" + categoryId, "GET", null);
            CategoryLibrary categoryLibrary = new CategoryLibrary();
            if (response != null)
            {
                BaseResponse<CategoryLibrary> baseResponse = new BaseResponse<CategoryLibrary>();
                baseResponse = baseResponse.JsonParseRecord(response);
                categoryLibrary = baseResponse.Data as CategoryLibrary;
            }
            return categoryLibrary;
        }

        public CommissionChargesLibrary SpecificCategoryBySellerandBrand(int categoryId, string SellerId, int brandId, string URL)
        {
            CommissionChargesLibrary chargesLibrary = new CommissionChargesLibrary();
            var response = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + categoryId + "&SellerID=" + SellerId + "&BrandID=" + brandId + "&onlyBrands=" + true, "GET", null);
            if (response != null)
            {
                baseResponse = new BaseResponse<CommissionChargesLibrary>();
                baseResponse = baseResponse.JsonParseRecord(response);
                if (baseResponse.code == 200)
                {
                    chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                }
                else
                {
                    CategoryLibrary category = getCategories(categoryId, URL);

                    if (category != null)
                    {
                        if (category.ParentId != null)
                        {
                            int parentCategegory = Convert.ToInt32(category.ParentId);

                            for (int i = 1; i <= category.CurrentLevel; i++)
                            {
                                CategoryLibrary Parentlibrary = getCategories(Convert.ToInt32(parentCategegory), URL);
                                if (Parentlibrary != null)
                                {

                                    var Parentresponse = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + Parentlibrary.Id + "&SellerID=" + SellerId + "&BrandID=" + brandId + "&onlyBrands=" + true, "GET", null);
                                    if (Parentresponse != null)
                                    {
                                        baseResponse = new BaseResponse<CommissionChargesLibrary>();
                                        baseResponse = baseResponse.JsonParseRecord(Parentresponse);
                                        if (baseResponse.code == 200)
                                        {
                                            chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                                            break;
                                        }
                                        else
                                        {
                                            if (Parentlibrary.ParentId != null)
                                            {
                                                parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                            }
                                            else
                                            {
                                                chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Parentlibrary.ParentId != null)
                                        {
                                            parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                        }
                                        else
                                        {
                                            chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                                    break;
                                }
                            }

                        }
                        else
                        {
                            chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                        }
                    }
                    else
                    {
                        chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                    }
                }
            }
            else
            {
                CategoryLibrary category = getCategories(categoryId, URL);

                if (category != null)
                {
                    if (category.ParentId != null)
                    {
                        int parentCategegory = Convert.ToInt32(category.ParentId);

                        for (int i = 1; i <= category.CurrentLevel; i++)
                        {
                            CategoryLibrary Parentlibrary = getCategories(Convert.ToInt32(parentCategegory), URL);
                            if (Parentlibrary != null)
                            {

                                var Parentresponse = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + Parentlibrary.Id + "&SellerID=" + SellerId + "&BrandID=" + brandId + "&onlyBrands=" + true, "GET", null);
                                if (Parentresponse != null)
                                {
                                    baseResponse = new BaseResponse<CommissionChargesLibrary>();
                                    baseResponse = baseResponse.JsonParseRecord(Parentresponse);
                                    if (baseResponse.code == 200)
                                    {
                                        chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                                        break;
                                    }
                                    else
                                    {
                                        if (Parentlibrary.ParentId != null)
                                        {
                                            parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                        }
                                        else
                                        {
                                            chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Parentlibrary.ParentId != null)
                                    {
                                        parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                    }
                                    else
                                    {
                                        chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                                break;
                            }
                        }

                    }
                    else
                    {
                        chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                    }
                }
                else
                {
                    chargesLibrary = SpecificCategoryBySeller(categoryId, SellerId, URL);
                }
            }
            return chargesLibrary;
        }

        public CommissionChargesLibrary SpecificCategoryBySeller(int categoryId, string SellerId, string URL)
        {
            CommissionChargesLibrary chargesLibrary = new CommissionChargesLibrary();
            var response = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + categoryId + "&SellerID=" + SellerId + "&onlySeller=" + true, "GET", null);
            if (response != null)
            {
                baseResponse = new BaseResponse<CommissionChargesLibrary>();
                baseResponse = baseResponse.JsonParseRecord(response);
                if (baseResponse.code == 200)
                {
                    chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                }
                else
                {
                    CategoryLibrary category = getCategories(categoryId, URL);

                    if (category != null)
                    {
                        if (category.ParentId != null)
                        {
                            int parentCategegory = Convert.ToInt32(category.ParentId);

                            for (int i = 1; i <= category.CurrentLevel; i++)
                            {
                                CategoryLibrary Parentlibrary = getCategories(Convert.ToInt32(parentCategegory), URL);
                                if (Parentlibrary != null)
                                {

                                    var Parentresponse = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + Parentlibrary.Id + "&SellerID=" + SellerId + "&onlySeller=" + true, "GET", null);
                                    if (Parentresponse != null)
                                    {
                                        baseResponse = new BaseResponse<CommissionChargesLibrary>();
                                        baseResponse = baseResponse.JsonParseRecord(Parentresponse);
                                        if (baseResponse.code == 200)
                                        {
                                            chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                                            break;
                                        }
                                        else
                                        {
                                            if (Parentlibrary.ParentId != null)
                                            {
                                                parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                            }
                                            else
                                            {
                                                chargesLibrary = SpecificCategory(categoryId, URL);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Parentlibrary.ParentId != null)
                                        {
                                            parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                        }
                                        else
                                        {
                                            chargesLibrary = SpecificCategory(categoryId, URL);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    chargesLibrary = SpecificCategory(categoryId, URL);
                                    break;
                                }
                            }

                        }
                        else
                        {
                            chargesLibrary = SpecificCategory(categoryId, URL);
                        }
                    }
                    else
                    {
                        chargesLibrary = SpecificCategory(categoryId, URL);
                    }
                }
            }
            else
            {
                CategoryLibrary category = getCategories(categoryId, URL);

                if (category != null)
                {
                    if (category.ParentId != null)
                    {
                        int parentCategegory = Convert.ToInt32(category.ParentId);

                        for (int i = 1; i <= category.CurrentLevel; i++)
                        {
                            CategoryLibrary Parentlibrary = getCategories(Convert.ToInt32(parentCategegory), URL);
                            if (Parentlibrary != null)
                            {

                                var Parentresponse = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + Parentlibrary.Id + "&SellerID=" + SellerId + "&onlySeller=" + true, "GET", null);
                                if (Parentresponse != null)
                                {
                                    baseResponse = new BaseResponse<CommissionChargesLibrary>();
                                    baseResponse = baseResponse.JsonParseRecord(Parentresponse);
                                    if (baseResponse.code == 200)
                                    {
                                        chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                                        break;
                                    }
                                    else
                                    {
                                        if (Parentlibrary.ParentId != null)
                                        {
                                            parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                        }
                                        else
                                        {
                                            chargesLibrary = SpecificCategory(categoryId, URL);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Parentlibrary.ParentId != null)
                                    {
                                        parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                    }
                                    else
                                    {
                                        chargesLibrary = SpecificCategory(categoryId, URL);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                chargesLibrary = SpecificCategory(categoryId, URL);
                                break;
                            }
                        }

                    }
                    else
                    {
                        chargesLibrary = SpecificCategory(categoryId, URL);
                    }
                }
                else
                {
                    chargesLibrary = SpecificCategory(categoryId, URL);
                }
            }
            return chargesLibrary;
        }

        public CommissionChargesLibrary SpecificCategory(int categoryId, string URL)
        {
            CommissionChargesLibrary chargesLibrary = new CommissionChargesLibrary();
            var response = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + categoryId + "&onlyCategory=" + true, "GET", null);
            if (response != null)
            {
                baseResponse = new BaseResponse<CommissionChargesLibrary>();
                baseResponse = baseResponse.JsonParseRecord(response);
                if (baseResponse.code == 200)
                {
                    chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                }
                else
                {
                    CategoryLibrary category = getCategories(categoryId, URL);

                    if (category != null)
                    {
                        if (category.ParentId != null)
                        {
                            int parentCategegory = Convert.ToInt32(category.ParentId);

                            for (int i = 1; i <= category.CurrentLevel; i++)
                            {
                                CategoryLibrary Parentlibrary = getCategories(Convert.ToInt32(parentCategegory), URL);
                                if (Parentlibrary != null)
                                {

                                    var Parentresponse = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + Parentlibrary.Id + "&onlyCategory=" + true, "GET", null);
                                    if (Parentresponse != null)
                                    {
                                        baseResponse = new BaseResponse<CommissionChargesLibrary>();
                                        baseResponse = baseResponse.JsonParseRecord(Parentresponse);
                                        if (baseResponse.code == 200)
                                        {
                                            chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                                            break;
                                        }
                                        else
                                        {
                                            if (Parentlibrary.ParentId != null)
                                            {
                                                parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Parentlibrary.ParentId != null)
                                        {
                                            parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                        }
                                    }
                                }
                                else
                                {
                                    break;
                                    //throw new Exception("Commission does not exist");
                                }
                            }

                        }
                        //else
                        //{
                        //    //throw new Exception("Commission does not exist");
                        //}
                    }
                    //else
                    //{
                    //    throw new Exception("Commission does not exist");
                    //}
                }
            }
            else
            {
                CategoryLibrary category = getCategories(categoryId, URL);

                if (category != null)
                {
                    if (category.ParentId != null)
                    {
                        int parentCategegory = Convert.ToInt32(category.ParentId);

                        for (int i = 1; i <= category.CurrentLevel; i++)
                        {
                            CategoryLibrary Parentlibrary = getCategories(Convert.ToInt32(parentCategegory), URL);
                            if (Parentlibrary != null)
                            {

                                var Parentresponse = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + Parentlibrary.Id + "&onlyCategory=" + true, "GET", null);
                                if (Parentresponse != null)
                                {
                                    baseResponse = new BaseResponse<CommissionChargesLibrary>();
                                    baseResponse = baseResponse.JsonParseRecord(Parentresponse);
                                    if (baseResponse.code == 200)
                                    {
                                        chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
                                        break;
                                    }
                                    else
                                    {
                                        if (Parentlibrary.ParentId != null)
                                        {
                                            parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                        }
                                    }
                                }
                                else
                                {
                                    if (Parentlibrary.ParentId != null)
                                    {
                                        parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
                                    }
                                }
                            }
                            else
                            {
                                break;
                                //throw new Exception("Commission does not exist");
                            }
                        }

                    }
                    //else
                    //{
                    //    throw new Exception("Commission does not exist");
                    //}
                }
                //else
                //{
                //    throw new Exception("Commission does not exist");
                //}
            }
            return chargesLibrary;
        }

        #region Comment Cumpulsary Code

        //public CommissionChargesLibrary CompulsaryCategoryBySellerandBrand(string SellerId, int brandId, string URL, string token, bool IsCompulsary)
        //{
        //    CommissionChargesLibrary chargesLibrary = new CommissionChargesLibrary();
        //    var response = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=" + IsCompulsary + "&SellerID=" + SellerId + "&BrandID=" + brandId + "&onlyBrands=" + true, "GET", null);
        //    if (response != null)
        //    {
        //        baseResponse = new BaseResponse<CommissionChargesLibrary>();
        //        baseResponse = baseResponse.JsonParseRecord(response);
        //        if (baseResponse.code == 200)
        //        {
        //            chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
        //        }
        //        else
        //        {
        //            chargesLibrary = CompulsaryCategoryBySeller(SellerId, URL, token, true);
        //        }
        //    }
        //    else
        //    {
        //        chargesLibrary = CompulsaryCategoryBySeller(SellerId, URL, token, true);
        //    }

        //    return chargesLibrary;
        //}
        //public CommissionChargesLibrary CompulsaryCategoryBySeller(string SellerId, string URL, string token, bool IsCompulsary)
        //{
        //    CommissionChargesLibrary chargesLibrary = new CommissionChargesLibrary();
        //    var response = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=" + IsCompulsary + "&SellerID=" + SellerId + "&onlySeller=" + true, "GET", null);
        //    if (response != null)
        //    {
        //        baseResponse = new BaseResponse<CommissionChargesLibrary>();
        //        baseResponse = baseResponse.JsonParseRecord(response);
        //        if (baseResponse.code == 200)
        //        {
        //            chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
        //        }
        //        else
        //        {
        //            chargesLibrary = CompulsaryCategory(URL, token, true);
        //        }
        //    }
        //    else
        //    {
        //        chargesLibrary = CompulsaryCategory(URL, token, true);
        //    }

        //    return chargesLibrary;
        //}

        //public CommissionChargesLibrary CompulsaryCategory(string URL, string token, bool IsCompulsary)
        //{
        //    CommissionChargesLibrary chargesLibrary = new CommissionChargesLibrary();
        //    var response = helper.ApiCall(URL, EndPoints.CommissionCharges + "?IsCompulsary=" + IsCompulsary + "&onlyCategory=" + true, "GET", null);
        //    if (response != null)
        //    {
        //        baseResponse = new BaseResponse<CommissionChargesLibrary>();
        //        baseResponse = baseResponse.JsonParseRecord(response);
        //        if (baseResponse.code == 200)
        //        {
        //            chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
        //        }
        //    }

        //    return chargesLibrary;
        //}

        //public CommissionChargesLibrary SpecificCategory(int categoryId, string URL, string token)
        //{
        //    CommissionChargesLibrary chargesLibrary = new CommissionChargesLibrary();
        //    var response = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + categoryId + "&onlyCategory=" + true, "GET", null);
        //    if (response != null)
        //    {
        //        baseResponse = new BaseResponse<CommissionChargesLibrary>();
        //        baseResponse = baseResponse.JsonParseRecord(response);
        //        if (baseResponse.code == 200)
        //        {
        //            chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
        //        }
        //        else
        //        {
        //            CategoryLibrary category = getCategories(categoryId, URL);

        //            if (category != null)
        //            {
        //                if (category.ParentId != null)
        //                {
        //                    int parentCategegory = Convert.ToInt32(category.ParentId);

        //                    for (int i = 1; i <= category.CurrentLevel; i++)
        //                    {
        //                        CategoryLibrary Parentlibrary = getCategories(Convert.ToInt32(parentCategegory), URL);
        //                        if (Parentlibrary != null)
        //                        {

        //                            var Parentresponse = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + Parentlibrary.Id + "&onlyCategory=" + true, "GET", null);
        //                            if (Parentresponse != null)
        //                            {
        //                                baseResponse = new BaseResponse<CommissionChargesLibrary>();
        //                                baseResponse = baseResponse.JsonParseRecord(Parentresponse);
        //                                if (baseResponse.code == 200)
        //                                {
        //                                    chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
        //                                    break;
        //                                }
        //                                else
        //                                {
        //                                    chargesLibrary = CompulsaryCategory(URL, token, false);
        //                                    break;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (Parentlibrary.ParentId != null)
        //                                {
        //                                    parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
        //                                }
        //                                else
        //                                {
        //                                    chargesLibrary = CompulsaryCategory(URL, token, false);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            chargesLibrary = CompulsaryCategory(URL, token, false);
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    chargesLibrary = CompulsaryCategory(URL, token, false);
        //                }
        //            }
        //            else
        //            {
        //                chargesLibrary = CompulsaryCategory(URL, token, false);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        CategoryLibrary category = getCategories(categoryId, URL);

        //        if (category != null)
        //        {
        //            if (category.ParentId != null)
        //            {
        //                int parentCategegory = Convert.ToInt32(category.ParentId);

        //                for (int i = 1; i <= category.CurrentLevel; i++)
        //                {
        //                    CategoryLibrary Parentlibrary = getCategories(Convert.ToInt32(parentCategegory), URL);
        //                    if (Parentlibrary != null)
        //                    {

        //                        var Parentresponse = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + Parentlibrary.Id + "&onlyCategory=" + true, "GET", null);
        //                        if (Parentresponse != null)
        //                        {
        //                            baseResponse = new BaseResponse<CommissionChargesLibrary>();
        //                            baseResponse = baseResponse.JsonParseList(Parentresponse);
        //                            chargesLibrary = baseResponse.Data as CommissionChargesLibrary;
        //                            break;
        //                        }
        //                        else
        //                        {
        //                            if (Parentlibrary.ParentId != null)
        //                            {
        //                                parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
        //                            }
        //                            else
        //                            {
        //                                chargesLibrary = CompulsaryCategory(URL, token, false);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        chargesLibrary = CompulsaryCategory(URL, token, false);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                chargesLibrary = CompulsaryCategory(URL, token, false);
        //            }
        //        }
        //        else
        //        {
        //            chargesLibrary = CompulsaryCategory(URL, token, false);
        //        }
        //    }
        //    return chargesLibrary;
        //}

        #endregion Comment Cumpulsary Code

        public bool AvailableCommissionInSpecificCategory(int categoryId, string URL, bool isCategoryWise=true)
        {
            bool _available = false;

            CategoryLibrary category = getCategories(categoryId, URL);
            if (category != null)
            {
                string[] paths = isCategoryWise ? category.ParentPathIds.Split('>') : category.PathIds.Split('>');
                for (int i = 0; i < paths.Length; i++)
                {
                    if (!string.IsNullOrEmpty(paths[i].ToString()))
                    {
                        var response = helper.ApiCall(URL, EndPoints.CommissionCharges + "?isCompulsary=false&CategoryID=" + paths[i] + "&onlyCategory=" + true, "GET", null);
                        baseResponse = new BaseResponse<CommissionChargesLibrary>();
                        baseResponse = baseResponse.JsonParseList(response);
                        List<CommissionChargesLibrary> Cattemplist = baseResponse.Data as List<CommissionChargesLibrary>;
                        if (Cattemplist.Count > 0)
                        {
                            _available = true;
                            break;
                        }
                    }
                }
            }
            
            return _available;
        }
    }
}

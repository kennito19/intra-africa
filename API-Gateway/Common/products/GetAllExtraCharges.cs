using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Http;

namespace API_Gateway.Common.products
{
    public class GetAllExtraCharges
    {
        
        BaseResponse<ExtraChargesLibrary> baseResponse = new BaseResponse<ExtraChargesLibrary>();
        private readonly HttpContext _httpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ApiHelper helper;
        public GetAllExtraCharges(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
        }

        public List<ExtraChargesLibrary> GetExtraCharges(int CategoryId, string URL)
        {
            //var extraCharges = helper.ApiCall(URL, EndPoints.ExtraCharges + "?PageIndex=" + 0 + "&PageSize=" + 0, "GET", null);
            //baseResponse = new BaseResponse<ExtraChargesLibrary>();
            //baseResponse = baseResponse.JsonParseList(extraCharges);
            //List<ExtraChargesLibrary> lstUniqueExtracharges = new List<ExtraChargesLibrary>();
            //lstUniqueExtracharges = (List<ExtraChargesLibrary>)baseResponse.Data;
            //lstUniqueExtracharges = lstUniqueExtracharges.GroupBy(p => p.Name).Select(p => p.First()).ToList();

            //List<ExtraChargesLibrary> lstExtracharges = new List<ExtraChargesLibrary>();
            //foreach (var charge in lstUniqueExtracharges)
            //{
            //    List<ExtraChargesLibrary> _Extracharges = new List<ExtraChargesLibrary>();
            //    ExtraChargesLibrary Extracharges = new ExtraChargesLibrary();

            //    //_Extracharges = CompulsaryCategory(charge.Name, URL, token, true);

            //    //if (_Extracharges.Count<=0 && CategoryId!=0)
            //    //{
            //    //}
            //    _Extracharges = SpecificCategory(CategoryId, charge.Name, URL);

            //    if (_Extracharges.Count > 0)
            //    {
            //        Extracharges = _Extracharges.FirstOrDefault();
            //        lstExtracharges.Add(Extracharges);

            //    }

            //}

            List<ExtraChargesLibrary> lstExtracharges = new List<ExtraChargesLibrary>();
            var extraCharges = helper.ApiCall(URL, EndPoints.CatExtraCharges + "?CategoryId=" + CategoryId, "GET", null);
            baseResponse = new BaseResponse<ExtraChargesLibrary>();
            baseResponse = baseResponse.JsonParseList(extraCharges);

            lstExtracharges = (List<ExtraChargesLibrary>)baseResponse.Data;
                
            return lstExtracharges;
        }

        public CategoryLibrary getCategories(int categoryId, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?Id=" + categoryId, "GET", null);
            CategoryLibrary categoryLibrary = new CategoryLibrary();
            if (response != null)
            {
                BaseResponse<CategoryLibrary> baseResponse = new BaseResponse<CategoryLibrary>();
                baseResponse = baseResponse.JsonParseRecord(response);
                if (baseResponse.code == 200)
                {
                    categoryLibrary = (CategoryLibrary)baseResponse.Data;
                }
            }
            return categoryLibrary;
        }

        public List<ExtraChargesLibrary> SpecificCategory(int categoryId, string Name, string URL)
        {
            List<ExtraChargesLibrary> chargesLibrary = new List<ExtraChargesLibrary>();
            var response = helper.ApiCall(URL, EndPoints.ExtraCharges + "?isCompulsary=false&catId=" + categoryId + "&name=" + Name, "GET", null);
            if (response != null)
            {
                baseResponse = new BaseResponse<ExtraChargesLibrary>();
                baseResponse = baseResponse.JsonParseList(response);
                if (baseResponse.code == 200)
                {
                    chargesLibrary = (List<ExtraChargesLibrary>)baseResponse.Data;
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

                                    var Parentresponse = helper.ApiCall(URL, EndPoints.ExtraCharges + "?IsCompulsary=false&CatID=" + Parentlibrary.Id, "GET", null);
                                    if (Parentresponse != null)
                                    {
                                        baseResponse = new BaseResponse<ExtraChargesLibrary>();
                                        baseResponse = baseResponse.JsonParseList(Parentresponse);
                                        if (baseResponse.code == 200)
                                        {
                                            chargesLibrary = (List<ExtraChargesLibrary>)baseResponse.Data;
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
                            }

                        }
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

                                var Parentresponse = helper.ApiCall(URL, EndPoints.ExtraCharges + "?IsCompulsary=false&CatID=" + Parentlibrary.Id, "GET", null);
                                if (Parentresponse != null)
                                {
                                    baseResponse = new BaseResponse<ExtraChargesLibrary>();
                                    baseResponse = baseResponse.JsonParseList(Parentresponse);
                                    if (baseResponse.code == 200)
                                    {
                                        chargesLibrary = (List<ExtraChargesLibrary>)baseResponse.Data;
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
                        }

                    }
                }
            }
            return chargesLibrary;
        }

        //public List<ExtraChargesLibrary> CompulsaryCategory(string Name, string URL, string token, bool IsCompulsary)
        //{
        //    List<ExtraChargesLibrary> chargesLibrary = new List<ExtraChargesLibrary>();
        //    var response = helper.ApiCall(URL, EndPoints.ExtraCharges + "?isCompulsary=" + IsCompulsary + "&name=" + Name, "GET", null);
        //    if (response != null)
        //    {
        //        baseResponse = new BaseResponse<ExtraChargesLibrary>();
        //        baseResponse = baseResponse.JsonParseList(response);
        //        if (baseResponse.code == 200)
        //        {
        //            chargesLibrary = (List<ExtraChargesLibrary>)baseResponse.Data;
        //        }
        //    }

        //    return chargesLibrary;
        //}

        //public List<ExtraChargesLibrary> SpecificCategory(int categoryId, string Name, string URL, string token)
        //{
        //    List<ExtraChargesLibrary> chargesLibrary = new List<ExtraChargesLibrary>();
        //    var response = helper.ApiCall(URL, EndPoints.ExtraCharges + "?isCompulsary=false&catId=" + categoryId+ "&name=" + Name, "GET", null);
        //    if (response != null)
        //    {
        //        baseResponse = new BaseResponse<ExtraChargesLibrary>();
        //        baseResponse = baseResponse.JsonParseList(response); 
        //        if (baseResponse.code == 200)
        //        {
        //            chargesLibrary = (List<ExtraChargesLibrary>)baseResponse.Data;
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

        //                        var Parentresponse = helper.ApiCall(URL, EndPoints.ExtraCharges + "?IsCompulsary=false&CatID=" + Parentlibrary.Id, "GET", null);
        //                        if (Parentresponse != null)
        //                        {
        //                            baseResponse = new BaseResponse<ExtraChargesLibrary>();
        //                            baseResponse = baseResponse.JsonParseList(Parentresponse);
        //                            if (baseResponse.code == 200)
        //                            {
        //                                chargesLibrary = (List<ExtraChargesLibrary>)baseResponse.Data;
        //                                break;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (Parentlibrary.ParentId != null)
        //                            {
        //                                parentCategegory = Convert.ToInt32(Parentlibrary.ParentId);
        //                            }
        //                            else
        //                            {
        //                                chargesLibrary = CompulsaryCategory(Name,URL, token, false);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        chargesLibrary = CompulsaryCategory(Name,URL, token, false);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                chargesLibrary = CompulsaryCategory(Name, URL, token, false);
        //            }
        //        }
        //        else
        //        {
        //            chargesLibrary = CompulsaryCategory(Name, URL, token, false);
        //        }
        //    }
        //    return chargesLibrary;
        //}
    }
}

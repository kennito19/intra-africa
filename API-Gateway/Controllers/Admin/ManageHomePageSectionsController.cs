using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageHomePageSectionsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string UserURL = string.Empty;
        BaseResponse<ManageHomePageSectionsLibrary> baseResponse = new BaseResponse<ManageHomePageSectionsLibrary>();
        private ApiHelper helper;
        public ManageHomePageSectionsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(ManageHomePageSectionsDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Name=" + model.Name + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHomePageSectionsLibrary> tempList = baseResponse.Data as List<ManageHomePageSectionsLibrary> ?? new List<ManageHomePageSectionsLibrary>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageHomePageSectionsLibrary mhps = new ManageHomePageSectionsLibrary();
                mhps.LayoutId = model.LayoutId;
                mhps.LayoutTypeId = model.LayoutTypeId;
                mhps.Name = model.Name;
                mhps.Sequence = model.Sequence;
                mhps.SectionColumns = model.SectionColumns;
                mhps.IsTitleVisible = model.IsTitleVisible;
                mhps.Title = model.Title;
                mhps.SubTitle = model.SubTitle;
                mhps.TitlePosition = model.TitlePosition;
                mhps.LinkIn = model.LinkIn;
                mhps.LinkText = model.LinkText;
                mhps.Link = model.Link;
                mhps.LinkPosition = model.LinkPosition;
                mhps.Status = model.Status;
                mhps.ListType = null;
                mhps.TopProducts = null;
                mhps.TotalRowsInSection = model.TotalRowsInSection;
                mhps.IsCustomGrid = model.IsCustomGrid;
                mhps.NumberOfImages = model.NumberOfImages;
                mhps.Column1 = model.Column1;
                mhps.Column2 = model.Column2;
                mhps.Column3 = model.Column3;
                mhps.Column4 = model.Column4;
                mhps.CategoryId = null;
                mhps.BackgroundColor = model.BackgroundColor;
                mhps.InContainer = model.InContainer;
                mhps.TitleColor = model.TitleColor;
                mhps.TextColor = model.TextColor;
                mhps.CreatedAt = DateTime.Now;
                mhps.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageHomePageSections, "POST", mhps);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ManageHomePageSectionsDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Name=" + model.Name + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHomePageSectionsLibrary> tempList = baseResponse.Data as List<ManageHomePageSectionsLibrary> ?? new List<ManageHomePageSectionsLibrary>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageHomePageSectionsLibrary mhps = baseResponse.Data as ManageHomePageSectionsLibrary;
                mhps.LayoutId = model.LayoutId;
                mhps.LayoutTypeId = model.LayoutTypeId;
                mhps.Name = model.Name;
                mhps.Sequence = model.Sequence;
                mhps.SectionColumns = model.SectionColumns;
                mhps.IsTitleVisible = model.IsTitleVisible;
                mhps.Title = model.Title;
                mhps.SubTitle = model.SubTitle;
                mhps.TitlePosition = model.TitlePosition;
                mhps.LinkIn = model.LinkIn;
                mhps.LinkText = model.LinkText;
                mhps.Link = model.Link;
                mhps.LinkPosition = model.LinkPosition;
                mhps.Status = model.Status;
                mhps.ListType = null;
                mhps.TopProducts = null;
                mhps.TotalRowsInSection = model.TotalRowsInSection;
                mhps.IsCustomGrid = model.IsCustomGrid;
                mhps.NumberOfImages = model.NumberOfImages;
                mhps.Column1 = model.Column1;
                mhps.Column2 = model.Column2;
                mhps.Column3 = model.Column3;
                mhps.Column4 = model.Column4;
                mhps.CategoryId = null;
                mhps.BackgroundColor = model.BackgroundColor;
                mhps.InContainer = model.InContainer;
                mhps.TitleColor = model.TitleColor;
                mhps.TextColor = model.TextColor;
                mhps.ModifiedAt = DateTime.Now;
                mhps.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageHomePageSections, "PUT", mhps);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }



        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHomePageSectionsLibrary> tempList = baseResponse.Data as List<ManageHomePageSectionsLibrary> ?? new List<ManageHomePageSectionsLibrary>();
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?SectionId=" + id, "GET", null);
                BaseResponse<ManageHomePageDetailsLibrary> baseResponse1 = new BaseResponse<ManageHomePageDetailsLibrary>();
                baseResponse1 = baseResponse1.JsonParseList(response);
                List<ManageHomePageDetailsLibrary> tempList1 = baseResponse1.Data as List<ManageHomePageDetailsLibrary> ?? new List<ManageHomePageDetailsLibrary>();
                if (tempList1.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    response = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpPost("CreateProductSection")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> CreateProductSection(ManageHomepageProductSectionDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Name=" + model.Name + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHomePageSectionsLibrary> tempList = baseResponse.Data as List<ManageHomePageSectionsLibrary> ?? new List<ManageHomePageSectionsLibrary>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageHomePageSectionsLibrary mhps = new ManageHomePageSectionsLibrary();
                mhps.LayoutId = model.LayoutId;
                mhps.LayoutTypeId = model.LayoutTypeId;
                mhps.Name = model.Name;
                mhps.Sequence = model.Sequence;
                mhps.SectionColumns = model.SectionColumns;
                mhps.Title = model.Title;
                mhps.SubTitle = model.SubTitle;
                mhps.LinkText = model.LinkText;
                mhps.Link = model.Link;
                mhps.Status = model.Status;
                mhps.ListType = model.ListType;
                mhps.TopProducts = model.TopProducts;
                mhps.IsTitleVisible = model.IsTitleVisible;
                mhps.TitlePosition = model.TitlePosition;
                mhps.LinkIn = model.LinkIn;
                mhps.LinkPosition = model.LinkPosition;
                mhps.BackgroundColor = model.BackgroundColor;
                mhps.InContainer = model.InContainer;
                mhps.TitleColor = model.TitleColor;
                mhps.TextColor = model.TextColor;
                mhps.CategoryId = model.CategoryId;
                mhps.CreatedAt = DateTime.Now;
                mhps.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageHomePageSections, "POST", mhps);
                baseResponse = baseResponse.JsonParseInputResponse(response);

                if (model.productSections.Count() > 0 && baseResponse.code == 200)
                {
                    int Count = 0;
                    int id = Convert.ToInt32(baseResponse.Data);
                    foreach (productSection item in model.productSections)
                    {
                        Count = Count + 1;
                        ManageHomePageDetailsLibrary manageHomePageDetails = new ManageHomePageDetailsLibrary();
                        manageHomePageDetails.SectionId = id;
                        manageHomePageDetails.LayoutTypeDetailsId = null;
                        manageHomePageDetails.Image = null;
                        manageHomePageDetails.ImageAlt = null;
                        manageHomePageDetails.Sequence = Count;
                        manageHomePageDetails.RedirectTo = "Specific product";
                        manageHomePageDetails.CategoryId = null;
                        manageHomePageDetails.BrandIds = null;
                        manageHomePageDetails.SizeIds = null;
                        manageHomePageDetails.SpecificationIds = null;
                        manageHomePageDetails.ColorIds = null;
                        manageHomePageDetails.CollectionId = null;
                        manageHomePageDetails.ProductId = item.productId;
                        manageHomePageDetails.StaticPageId = null;
                        manageHomePageDetails.LendingPageId = null;
                        manageHomePageDetails.CustomLinks = null;
                        manageHomePageDetails.AssignCity = item.AssignCity;
                        manageHomePageDetails.AssignState = item.AssignState;
                        manageHomePageDetails.AssignCountry = item.AssignCountry;
                        manageHomePageDetails.Status = "Active";
                        manageHomePageDetails.CreatedAt = DateTime.Now;
                        manageHomePageDetails.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                        try
                        {
                            var Addresponse = helper.ApiCall(URL, EndPoints.ManageHomePageDetails, "POST", manageHomePageDetails);
                            baseResponse = baseResponse.JsonParseInputResponse(Addresponse);
                        }
                        catch (Exception ex)
                        {

                            throw new Exception(ex.Message);
                        }

                    }
                }
            }

            return Ok(baseResponse);
        }

        [HttpPut("UpdateProductSection")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> UpdateProductSection(ManageHomepageProductSectionDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Name=" + model.Name + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHomePageSectionsLibrary> tempList = baseResponse.Data as List<ManageHomePageSectionsLibrary> ?? new List<ManageHomePageSectionsLibrary>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageHomePageSectionsLibrary mhps = baseResponse.Data as ManageHomePageSectionsLibrary;
                mhps.LayoutId = model.LayoutId;
                mhps.LayoutTypeId = model.LayoutTypeId;
                mhps.Name = model.Name;
                mhps.Sequence = model.Sequence;
                mhps.SectionColumns = model.SectionColumns;
                mhps.Title = model.Title;
                mhps.SubTitle = model.SubTitle;
                mhps.LinkText = model.LinkText;
                mhps.Link = model.Link;
                mhps.Status = model.Status;
                mhps.ListType = model.ListType;
                mhps.TopProducts = model.TopProducts;
                mhps.CategoryId = model.CategoryId;
                mhps.IsTitleVisible = model.IsTitleVisible;
                mhps.TitlePosition = model.TitlePosition;
                mhps.LinkIn = model.LinkIn;
                mhps.LinkPosition = model.LinkPosition;
                mhps.BackgroundColor = model.BackgroundColor;
                mhps.InContainer = model.InContainer;
                mhps.TitleColor = model.TitleColor;
                mhps.TextColor = model.TextColor;
                mhps.ModifiedAt = DateTime.Now;
                mhps.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageHomePageSections, "PUT", mhps);
                baseResponse = baseResponse.JsonParseInputResponse(response);

                if (model.productSections.Count() > 0)
                {
                    BaseResponse<ManageHomePageDetailsLibrary> baseResponseHomedetails = new BaseResponse<ManageHomePageDetailsLibrary>();

                    var responsedetails = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?LayoutTypeDetailsId=" + 0 + "&sectionId=" + model.Id, "GET", null);
                    baseResponseHomedetails = baseResponseHomedetails.JsonParseList(responsedetails);

                    if (baseResponseHomedetails.code == 200)
                    {
                        List<ManageHomePageDetailsLibrary> manageHomePageDetailsLibraries = baseResponseHomedetails.Data as List<ManageHomePageDetailsLibrary> ?? new List<ManageHomePageDetailsLibrary>();

                        if (manageHomePageDetailsLibraries.Count > 0)
                        {
                            var deleteresponse = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?SectionId=" + model.Id, "DELETE", null);
                            baseResponseHomedetails = baseResponseHomedetails.JsonParseInputResponse(deleteresponse);
                            if (baseResponseHomedetails.code == 200)
                            {
                                int Count = 0;
                                foreach (productSection item in model.productSections)
                                {
                                    Count = Count + 1;
                                    ManageHomePageDetailsLibrary manageHomePageDetails = new ManageHomePageDetailsLibrary();
                                    manageHomePageDetails.SectionId = model.Id;
                                    manageHomePageDetails.LayoutTypeDetailsId = null;
                                    manageHomePageDetails.Image = null;
                                    manageHomePageDetails.ImageAlt = null;
                                    manageHomePageDetails.Sequence = Count;
                                    manageHomePageDetails.RedirectTo = "Specific product";
                                    manageHomePageDetails.CategoryId = null;
                                    manageHomePageDetails.BrandIds = null;
                                    manageHomePageDetails.SizeIds = null;
                                    manageHomePageDetails.SpecificationIds = null;
                                    manageHomePageDetails.ColorIds = null;
                                    manageHomePageDetails.CollectionId = null;
                                    manageHomePageDetails.ProductId = item.productId;
                                    manageHomePageDetails.StaticPageId = null;
                                    manageHomePageDetails.LendingPageId = null;
                                    manageHomePageDetails.CustomLinks = null;
                                    manageHomePageDetails.AssignCity = item.AssignCity;
                                    manageHomePageDetails.AssignState = item.AssignState;
                                    manageHomePageDetails.AssignCountry = item.AssignCountry;
                                    manageHomePageDetails.Status = "Active";
                                    manageHomePageDetails.CreatedAt = DateTime.Now;
                                    manageHomePageDetails.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                                    try
                                    {
                                        var Addresponse = helper.ApiCall(URL, EndPoints.ManageHomePageDetails, "POST", manageHomePageDetails);
                                        baseResponse = baseResponse.JsonParseInputResponse(Addresponse);
                                    }
                                    catch (Exception ex)
                                    {

                                        throw new Exception(ex.Message);
                                    }

                                }


                            }
                        }
                    }
                }
            }

            return Ok(baseResponse);
        }

        [HttpDelete("DeleteSection")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> DeleteSection(int sectionId)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Id=" + sectionId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHomePageSectionsLibrary> tempList = baseResponse.Data as List<ManageHomePageSectionsLibrary> ?? new List<ManageHomePageSectionsLibrary>();
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?SectionId=" + sectionId, "GET", null);
                BaseResponse<ManageHomePageDetailsLibrary> baseResponse1 = new BaseResponse<ManageHomePageDetailsLibrary>();
                baseResponse1 = baseResponse1.JsonParseList(response);
                List<ManageHomePageDetailsLibrary> tempList1 = baseResponse1.Data as List<ManageHomePageDetailsLibrary> ?? new List<ManageHomePageDetailsLibrary>();
                if (tempList1.Any())
                {
                    foreach (var item in tempList1)
                    {
                        if (!string.IsNullOrEmpty(item.Image))
                        {
                            ImageUpload imageUpload = new ImageUpload(_configuration);
                            imageUpload.RemoveDocFile(item.Image, "HomePages");
                        }
                    }

                    BaseResponse<ManageHomePageDetailsLibrary> baseResponseHomedetails = new BaseResponse<ManageHomePageDetailsLibrary>();

                    var deleteresponse = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?SectionId=" + sectionId, "DELETE", null);
                    baseResponseHomedetails = baseResponseHomedetails.JsonParseInputResponse(deleteresponse);
                    if (baseResponseHomedetails.code == 200)
                    {
                        response = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Id=" + sectionId, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
                else
                {
                    response = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Id=" + sectionId, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<ApiHelper> Get(int? pageindex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?PageIndex=" + pageindex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize]
        public ActionResult<ApiHelper> Search(string? searchtext = null, string? status = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = "?PageIndex=" + pageIndex + "&PageSize=" + pageSize;
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                url += "&SearchText=" + HttpUtility.UrlEncode(searchtext);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                url += "&Status=" + HttpUtility.UrlEncode(status);
            }

            var response = helper.ApiCall(URL, EndPoints.ManageHomePageSections + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("GetProductHomePageSection")]
        [Authorize]
        public ActionResult<ApiHelper> GetProductHomePageSection(int? categoryId = 0, int? topProduct = 0, string? productId = null)
        {
            string userId = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault() != null ? HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value : null;
            BaseResponse<ProductHomePageSectionLibrary> productHomePage = new BaseResponse<ProductHomePageSectionLibrary>();
            productHomePage.Message = "Record bind successfully.";
            productHomePage.code = 200;
            List<Wishlist> lstwish = new List<Wishlist>();
            if (!string.IsNullOrEmpty(userId))
            {
                lstwish = GetUserWishlist(userId);
            }

            var response = helper.ApiCall(URL, EndPoints.ManageProductHomePageSections + "?categoryId=" + categoryId + "&topProduct=" + topProduct + "&productId=" + productId, "GET", null);
            var parsedResponse = productHomePage.JsonParseList(response);
            List<ProductHomePageSectionLibrary> homePageDetail = parsedResponse.Data as List<ProductHomePageSectionLibrary> ?? new List<ProductHomePageSectionLibrary>();
            var result = homePageDetail.Select(section => new
            {
                id = section.id,
                Guid = section.Guid,
                isMasterProduct = section.isMasterProduct,
                parentId = section.parentId,
                categoryId = section.categoryId,
                assiCategoryId = section.assiCategoryId,
                productName = section.productName,
                customeProductName = section.customeProductName,
                companySkuCode = section.companySkuCode,
                image1 = section.image1,
                mrp = section.mrp,
                sellingPrice = section.sellingPrice,
                discount = section.discount,
                quantity = section.Quantity,
                createdAt = section.createdAt,
                modifiedAt = section.modifiedAt,
                categoryName = section.categoryName,
                categoryPathId = section.categoryPathIds,
                categoryPathNames = section.categoryPathNames,
                sellerProductId = section.sellerProductId,
                sellerid = section.sellerId,
                sellerName = ExtractSellerNameFromExtraDetails(section.extraDetails),
                brandid = section.brandId,
                brandName = ExtractBrandNameFromExtraDetails(section.extraDetails),
                status = section.status,
                live = section.live,
                extraDetails = section.extraDetails,
                totalVariant = section.totalVariant,
                isWishlist = lstwish != null && lstwish.Count > 0 ? lstwish.Where(p => p.ProductId == section.Guid).ToList().Count > 0 ? true : false : false,
            }) ;
            productHomePage.Data = result;
            return Ok(productHomePage);
        }

        [NonAction]
        private string ExtractBrandNameFromExtraDetails(string extraDetails)
        {
            dynamic extraDetailsObj = JsonConvert.DeserializeObject(extraDetails);
            string brandName = extraDetailsObj.BrandDetails.Name;
            return brandName;
        }

        [NonAction]
        private string ExtractSellerNameFromExtraDetails(string extraDetails)
        {
            dynamic extraDetailsObj = JsonConvert.DeserializeObject(extraDetails);
            string sellerName = extraDetailsObj.SellerDetails.FullName;
            return sellerName;
        }

        [NonAction]
        private List<Wishlist> GetUserWishlist(string userId)
        {
            BaseResponse<Wishlist> baseResponse = new BaseResponse<Wishlist>();
            var response = helper.ApiCall(UserURL, EndPoints.Wishlist + "?UserID=" + userId + "&pageIndex=0&pageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<Wishlist> lstwish = baseResponse.Data as List<Wishlist> ?? new List<Wishlist>();

            return lstwish;
        }
    }
}

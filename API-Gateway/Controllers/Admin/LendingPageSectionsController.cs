using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendingPageSectionsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<LendingPageSections> baseResponse = new BaseResponse<LendingPageSections>();
        private ApiHelper helper;
        public LendingPageSectionsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(LendingPageSectionsDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Name=" + model.Name + "&LendingPageId=" + model.LendingPageId + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<LendingPageSections> tempList = baseResponse.Data as List<LendingPageSections> ?? new List<LendingPageSections>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                LendingPageSections manageLendingPageSection = new LendingPageSections();
                manageLendingPageSection.LendingPageId = model.LendingPageId;
                manageLendingPageSection.LayoutId = model.LayoutId;
                manageLendingPageSection.LayoutTypeId = model.LayoutTypeId;
                manageLendingPageSection.Name = model.Name;
                manageLendingPageSection.Sequence = model.Sequence;
                manageLendingPageSection.SectionColumns = model.SectionColumns;
                manageLendingPageSection.Title = model.Title;
                manageLendingPageSection.SubTitle = model.SubTitle;
                manageLendingPageSection.LinkText = model.LinkText;
                manageLendingPageSection.Link = model.Link;
                manageLendingPageSection.IsTitleVisible = model.IsTitleVisible;
                manageLendingPageSection.TitlePosition = model.TitlePosition;
                manageLendingPageSection.LinkIn = model.LinkIn;
                manageLendingPageSection.LinkPosition = model.LinkPosition;
                manageLendingPageSection.BackgroundColor = model.BackgroundColor;
                manageLendingPageSection.InContainer = model.InContainer;
                manageLendingPageSection.TitleColor = model.TitleColor;
                manageLendingPageSection.TextColor = model.TextColor;
                manageLendingPageSection.TotalRowsInSection = model.TotalRowsInSection;
                manageLendingPageSection.IsCustomGrid = model.IsCustomGrid;
                manageLendingPageSection.NumberOfImages = model.NumberOfImages;
                manageLendingPageSection.Column1 = model.Column1;
                manageLendingPageSection.Column2 = model.Column2;
                manageLendingPageSection.Column3 = model.Column3;
                manageLendingPageSection.Column4 = model.Column4;
                manageLendingPageSection.Status = model.Status;
                manageLendingPageSection.CreatedAt = DateTime.Now;
                manageLendingPageSection.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSection, "POST", manageLendingPageSection);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(LendingPageSectionsDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Name=" + model.Name + "&LendingPageId=" + model.LendingPageId + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<LendingPageSections> tempList = baseResponse.Data as List<LendingPageSections> ?? new List<LendingPageSections>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                LendingPageSections manageLendingPage = baseResponse.Data as LendingPageSections;
                manageLendingPage.Id = model.Id;
                manageLendingPage.LendingPageId = model.LendingPageId;
                manageLendingPage.LayoutId = model.LayoutId;
                manageLendingPage.LayoutTypeId = model.LayoutTypeId;
                manageLendingPage.Name = model.Name;
                manageLendingPage.Sequence = model.Sequence;
                manageLendingPage.SectionColumns = model.SectionColumns;
                manageLendingPage.Title = model.Title;
                manageLendingPage.SubTitle = model.SubTitle;
                manageLendingPage.LinkText = model.LinkText;
                manageLendingPage.Link = model.Link;
                manageLendingPage.IsTitleVisible = model.IsTitleVisible;
                manageLendingPage.TitlePosition = model.TitlePosition;
                manageLendingPage.LinkIn = model.LinkIn;
                manageLendingPage.LinkPosition = model.LinkPosition;
                manageLendingPage.BackgroundColor = model.BackgroundColor;
                manageLendingPage.InContainer = model.InContainer;
                manageLendingPage.TitleColor = model.TitleColor;
                manageLendingPage.TextColor = model.TextColor;
                manageLendingPage.TotalRowsInSection = model.TotalRowsInSection;
                manageLendingPage.IsCustomGrid = model.IsCustomGrid;
                manageLendingPage.NumberOfImages = model.NumberOfImages;
                manageLendingPage.Column1 = model.Column1;
                manageLendingPage.Column2 = model.Column2;
                manageLendingPage.Column3 = model.Column3;
                manageLendingPage.Column4 = model.Column4;
                manageLendingPage.Status = model.Status;
                manageLendingPage.ModifiedAt = DateTime.Now;
                manageLendingPage.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSection, "PUT", manageLendingPage);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPost("CreateProductSection")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> CreateProductSection(ManageLendingpageProductSectionDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Name=" + model.Name + "&LendingPageId=" + model.LendingPageId + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<LendingPageSections> tempList = baseResponse.Data as List<LendingPageSections> ?? new List<LendingPageSections>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                LendingPageSections manageLendingPageSection = new LendingPageSections();
                manageLendingPageSection.LendingPageId = model.LendingPageId;
                manageLendingPageSection.LayoutId = model.LayoutId;
                manageLendingPageSection.LayoutTypeId = model.LayoutTypeId;
                manageLendingPageSection.Name = model.Name;
                manageLendingPageSection.Sequence = model.Sequence;
                manageLendingPageSection.SectionColumns = model.SectionColumns;
                manageLendingPageSection.Title = model.Title;
                manageLendingPageSection.SubTitle = model.SubTitle;
                manageLendingPageSection.LinkText = model.LinkText;
                manageLendingPageSection.Link = model.Link;
                manageLendingPageSection.Status = model.Status;
                manageLendingPageSection.IsTitleVisible = model.IsTitleVisible;
                manageLendingPageSection.TitlePosition = model.TitlePosition;
                manageLendingPageSection.LinkIn = model.LinkIn;
                manageLendingPageSection.LinkPosition = model.LinkPosition;
                manageLendingPageSection.BackgroundColor = model.BackgroundColor;
                manageLendingPageSection.InContainer = model.InContainer;
                manageLendingPageSection.TitleColor = model.TitleColor;
                manageLendingPageSection.TextColor = model.TextColor;
                manageLendingPageSection.CreatedAt = DateTime.Now;
                manageLendingPageSection.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSection, "POST", manageLendingPageSection);
                baseResponse = baseResponse.JsonParseInputResponse(response);

                if (model.productSections.Count() > 0 && baseResponse.code == 200)
                {
                    int Count = 0;
                    int id = Convert.ToInt32(baseResponse.Data);

                    foreach (LendingPageproductSection item in model.productSections)
                    {
                        Count = Count + 1;
                        LendingPageSectionDetails LendingPageSectionDetail = new LendingPageSectionDetails();
                        LendingPageSectionDetail.LendingPageSectionId = id;
                        LendingPageSectionDetail.LayoutTypeDetailsId = null;
                        LendingPageSectionDetail.Image = null;
                        LendingPageSectionDetail.ImageAlt = null;
                        LendingPageSectionDetail.Sequence = Count;
                        LendingPageSectionDetail.RedirectTo = "Specific product";
                        LendingPageSectionDetail.CategoryId = null;
                        LendingPageSectionDetail.BrandIds = null;
                        LendingPageSectionDetail.SizeIds = null;
                        LendingPageSectionDetail.SpecificationIds = null;
                        LendingPageSectionDetail.ColorIds = null;
                        LendingPageSectionDetail.CollectionId = null;
                        LendingPageSectionDetail.ProductId = item.productId;
                        LendingPageSectionDetail.StaticPageId = null;
                        LendingPageSectionDetail.CustomLinks = null;
                        LendingPageSectionDetail.AssignCity = item.AssignCity;
                        LendingPageSectionDetail.AssignState = item.AssignState;
                        LendingPageSectionDetail.AssignCountry = item.AssignCountry;
                        LendingPageSectionDetail.Status = "Active";
                        LendingPageSectionDetail.CreatedAt = DateTime.Now;
                        LendingPageSectionDetail.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                        try
                        {
                            var Addresponse = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail, "POST", LendingPageSectionDetail);
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
        public ActionResult<ApiHelper> UpdateProductSection(ManageLendingpageProductSectionDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Name=" + model.Name + "&LendingPageId=" + model.LendingPageId + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<LendingPageSections> tempList = baseResponse.Data as List<LendingPageSections> ?? new List<LendingPageSections>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageLendingPage + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                LendingPageSections manageLendingPage = baseResponse.Data as LendingPageSections;
                manageLendingPage.Id = model.Id;
                manageLendingPage.LendingPageId = model.LendingPageId;
                manageLendingPage.LayoutId = model.LayoutId;
                manageLendingPage.LayoutTypeId = model.LayoutTypeId;
                manageLendingPage.Name = model.Name;
                manageLendingPage.Sequence = model.Sequence;
                manageLendingPage.SectionColumns = model.SectionColumns;
                manageLendingPage.Title = model.Title;
                manageLendingPage.SubTitle = model.SubTitle;
                manageLendingPage.LinkText = model.LinkText;
                manageLendingPage.Link = model.Link;
                manageLendingPage.Status = model.Status;
                manageLendingPage.IsTitleVisible = model.IsTitleVisible;
                manageLendingPage.TitlePosition = model.TitlePosition;
                manageLendingPage.LinkIn = model.LinkIn;
                manageLendingPage.LinkPosition = model.LinkPosition;
                manageLendingPage.BackgroundColor = model.BackgroundColor;
                manageLendingPage.InContainer = model.InContainer;
                manageLendingPage.TitleColor = model.TitleColor;
                manageLendingPage.TextColor = model.TextColor;
                manageLendingPage.ModifiedAt = DateTime.Now;
                manageLendingPage.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSection, "PUT", manageLendingPage);
                baseResponse = baseResponse.JsonParseInputResponse(response);

                if (model.productSections.Count() > 0)
                {
                    BaseResponse<LendingPageSectionDetails> baseResponseHomedetails = new BaseResponse<LendingPageSectionDetails>();

                    var responsedetails = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?LayoutTypeDetailsId=" + 0 + "&LendingPageSectionId=" + model.Id, "GET", null);
                    baseResponseHomedetails = baseResponseHomedetails.JsonParseList(responsedetails);

                    if (baseResponseHomedetails.code == 200)
                    {
                        List<LendingPageSectionDetails> lendingPageSectionDetailslst = baseResponseHomedetails.Data as List<LendingPageSectionDetails> ?? new List<LendingPageSectionDetails>();

                        if (lendingPageSectionDetailslst.Count > 0)
                        {
                            var deleteresponse = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?LendingPageSectionId=" + model.Id, "DELETE", null);
                            baseResponseHomedetails = baseResponseHomedetails.JsonParseInputResponse(deleteresponse);
                            if (baseResponseHomedetails.code == 200)
                            {
                                int Count = 0;
                                foreach (LendingPageproductSection item in model.productSections)
                                {
                                    Count = Count + 1;
                                    LendingPageSectionDetails LendingPageSectionDetail = new LendingPageSectionDetails();
                                    LendingPageSectionDetail.LendingPageSectionId = model.Id;
                                    LendingPageSectionDetail.LayoutTypeDetailsId = null;
                                    LendingPageSectionDetail.Image = null;
                                    LendingPageSectionDetail.ImageAlt = null;
                                    LendingPageSectionDetail.Sequence = Count;
                                    LendingPageSectionDetail.RedirectTo = "Specific product";
                                    LendingPageSectionDetail.CategoryId = null;
                                    LendingPageSectionDetail.BrandIds = null;
                                    LendingPageSectionDetail.SizeIds = null;
                                    LendingPageSectionDetail.SpecificationIds = null;
                                    LendingPageSectionDetail.ColorIds = null;
                                    LendingPageSectionDetail.CollectionId = null;
                                    LendingPageSectionDetail.ProductId = item.productId;
                                    LendingPageSectionDetail.StaticPageId = null;
                                    LendingPageSectionDetail.CustomLinks = null;
                                    LendingPageSectionDetail.AssignCity = item.AssignCity;
                                    LendingPageSectionDetail.AssignState = item.AssignState;
                                    LendingPageSectionDetail.AssignCountry = item.AssignCountry;
                                    LendingPageSectionDetail.Status = "Active";
                                    LendingPageSectionDetail.CreatedAt = DateTime.Now;
                                    LendingPageSectionDetail.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                                    try
                                    {
                                        var Addresponse = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail, "POST", LendingPageSectionDetail);
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
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Id=" + sectionId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<LendingPageSections> tempList = baseResponse.Data as List<LendingPageSections> ?? new List<LendingPageSections>();
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?LendingPageSectionId=" + sectionId, "GET", null);
                BaseResponse<LendingPageSectionDetails> baseResponse1 = new BaseResponse<LendingPageSectionDetails>();
                baseResponse1 = baseResponse1.JsonParseList(response);
                List<LendingPageSectionDetails> tempList1 = baseResponse1.Data as List<LendingPageSectionDetails> ?? new List<LendingPageSectionDetails>();
                if (tempList1.Any())
                {
                    foreach (var item in tempList1)
                    {
                        if (!string.IsNullOrEmpty(item.Image))
                        {
                            ImageUpload imageUpload = new ImageUpload(_configuration);
                            imageUpload.RemoveDocFile(item.Image, "LendingPageSections");
                        }
                    }

                    BaseResponse<LendingPageSectionDetails> baseResponseHomedetails = new BaseResponse<LendingPageSectionDetails>();

                    var deleteresponse = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?LendingPageSectionId=" + sectionId, "DELETE", null);
                    baseResponseHomedetails = baseResponseHomedetails.JsonParseInputResponse(deleteresponse);
                    if (baseResponseHomedetails.code == 200)
                    {
                        response = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Id=" + sectionId, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
                else
                {
                    response = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Id=" + sectionId, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<LendingPageSections> tempList = baseResponse.Data as List<LendingPageSections> ?? new List<LendingPageSections>();
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?LendingPageSectionId=" + id, "GET", null);
                BaseResponse<LendingPageSectionDetails> baseResponse1 = new BaseResponse<LendingPageSectionDetails>();
                baseResponse1 = baseResponse1.JsonParseList(response);
                List<LendingPageSectionDetails> tempList1 = baseResponse1.Data as List<LendingPageSectionDetails> ?? new List<LendingPageSectionDetails>();
                if (tempList1.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    response = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Id=" + id, "DELETE", null);
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
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("GetLendingPageSection")]
        [Authorize]
        public ActionResult<ApiHelper> GetLendingPageSection(int LendingPageId, string? status = null)
        {
            getLendingPageSections getHomePage = new getLendingPageSections(_configuration, _httpContext);
            JObject res = getHomePage.setSections(LendingPageId, status);
            return Ok(res.ToString());
        }


    }
}

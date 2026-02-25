using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Common
{
    public class Brands
    {
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string _URL = string.Empty;
        public static string catalougeURL = string.Empty;
        BaseResponse<BrandLibrary> baseResponse = new BaseResponse<BrandLibrary>();
        private ApiHelper helper;
        public Brands(string URL, IConfiguration configuration, HttpContext httpContext = null)
        {
            _URL = URL;
            _configuration = configuration;
            catalougeURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            _httpContext = httpContext;
            helper = new ApiHelper(_httpContext);
        }

        public BaseResponse<BrandLibrary> SaveBrand([FromForm] BrandDTO model, string UserId, bool IsAdmin)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Name))
                {
                    return baseResponse.InvalidInput("Brand name is required.");
                }

                var temp = helper.ApiCall(_URL, EndPoints.Brand + "?Name=" + model.Name.Replace("'", "''"), "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<BrandLibrary> tmp = baseResponse.Data as List<BrandLibrary> ?? new List<BrandLibrary>();
                if (tmp.Any())
                {
                    baseResponse = baseResponse.AlreadyExists();
                }
                else
                {
                    var temp2 = helper.ApiCall(_URL, EndPoints.Brand + "?Name=" + model.Name.Replace("'", "''") + "&isDeleted=" + true, "GET", null);
                    baseResponse = baseResponse.JsonParseList(temp2);
                    List<BrandLibrary> tmp2 = baseResponse.Data as List<BrandLibrary> ?? new List<BrandLibrary>();
                    if (tmp2.Count > 0)
                    {
                        var data = tmp2.FirstOrDefault();

                        BrandLibrary abts = new BrandLibrary();
                        abts.ID = data.ID;
                        abts.Name = model.Name;
                        abts.Description = model.Description;
                        if (IsAdmin)
                        {
                            abts.Status = model.Status;
                        }
                        else
                        {
                            abts.Status = "Request For Approval";
                        }
                        if (model.FileName != null)
                        {
                            abts.Logo = UploadDoc(model.Name, model.FileName);
                        }
                        else
                        {
                            abts.Logo = model.Logo;
                        }
                        abts.Logo = abts.Logo ?? string.Empty;
                        abts.CreatedBy = UserId;
                        abts.CreatedAt = DateTime.Now;

                        abts.ModifiedBy = null;
                        abts.ModifiedAt = null;

                        abts.IsDeleted = false;
                        abts.DeletedAt = null;
                        abts.DeletedBy = null;
                        var response = helper.ApiCall(_URL, EndPoints.Brand, "PUT", abts);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                        baseResponse.Data = abts.ID;
                    }
                    else
                    {
                        BrandLibrary abts = new BrandLibrary();
                        abts.Name = model.Name;
                        abts.Description = model.Description;
                        if (IsAdmin)
                        {
                            abts.Status = model.Status;
                        }
                        else
                        {
                            abts.Status = "Request For Approval";
                        }
                        if (model.FileName != null)
                        {
                            abts.Logo = UploadDoc(model.Name, model.FileName);
                        }
                        else
                        {
                            abts.Logo = model.Logo;
                        }
                        abts.Logo = abts.Logo ?? string.Empty;
                        abts.CreatedBy = UserId;
                        abts.CreatedAt = DateTime.Now;

                        var response = helper.ApiCall(_URL, EndPoints.Brand, "POST", abts);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
                return baseResponse;
            }
            catch (Exception ex)
            {
                return baseResponse.InvalidInput(ex.Message);
            }
        }

        public BaseResponse<BrandLibrary> UpdateBrand([FromForm] BrandDTO model, string UserId)
        {
            try
            {
                if (model == null || model.ID == null || model.ID == 0 || string.IsNullOrWhiteSpace(model.Name))
                {
                    return baseResponse.InvalidInput("Brand id and name are required.");
                }

                var temp = helper.ApiCall(_URL, EndPoints.Brand + "?Name=" + model.Name.Replace("'", "''"), "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<BrandLibrary> tmp = baseResponse.Data as List<BrandLibrary> ?? new List<BrandLibrary>();
                if (tmp.Where(x => x.ID != model.ID).Any())
                {
                    baseResponse = baseResponse.AlreadyExists();
                }
                else
                {
                    var response = helper.ApiCall(_URL, EndPoints.Brand + "?Id=" + model.ID, "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(response);
                    BrandLibrary abts = baseResponse.Data as BrandLibrary;
                    if (abts == null)
                    {
                        return baseResponse.NotExist();
                    }
                    string OldName = abts.Logo;
                    abts.Name = model.Name;
                    abts.Description = model.Description;
                    abts.Status = model.Status;
                    abts.Logo = UpdateDocFile(OldName, model.Name, model.FileName);
                    abts.Logo = abts.Logo ?? string.Empty;

                    abts.ModifiedBy = UserId;
                    abts.ModifiedAt = DateTime.Now;

                    abts.IsDeleted = false;
                    abts.DeletedAt = null;
                    abts.DeletedBy = null;

                    response = helper.ApiCall(_URL, EndPoints.Brand, "PUT", abts);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    if (baseResponse.code == 200)
                    {
                        BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();

                        ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                        productExtraDetails.BrandStatus = model.Status;
                        productExtraDetails.BrandName = model.Name;
                        productExtraDetails.BrandLogo = model.Logo;
                        productExtraDetails.BrandId = model.ID;
                        productExtraDetails.Mode = "updateBrands";
                        var ExtraDetailsresponse = helper.ApiCall(catalougeURL, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                        ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);
                    }
                }
                return baseResponse;
            }
            catch (Exception ex)
            {
                return baseResponse.InvalidInput(ex.Message);
            }
        }

        public BaseResponse<BrandLibrary> DeleteBrand(int? id = 0)
        {
            try
            {
                var temp = helper.ApiCall(_URL, EndPoints.Brand + "?id=" + id, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<BrandLibrary> templist = baseResponse.Data as List<BrandLibrary> ?? new List<BrandLibrary>();
                if (templist.Any())
                {
                    List<AssignBrandToSeller> brandToSeller = new List<AssignBrandToSeller>();
                    List<SellerProduct> sellerProducts = new List<SellerProduct>();

                    try
                    {
                        var tempAssignBrandToSeller = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?BrandId=" + id, "GET", null);
                        BaseResponse<AssignBrandToSeller> baseAssignBrandToSeller = new BaseResponse<AssignBrandToSeller>();
                        var assignBrandToSeller = baseAssignBrandToSeller.JsonParseList(tempAssignBrandToSeller);
                        brandToSeller = assignBrandToSeller.Data as List<AssignBrandToSeller> ?? new List<AssignBrandToSeller>();
                    }
                    catch
                    {
                        brandToSeller = new List<AssignBrandToSeller>();
                    }

                    try
                    {
                        var tempSellerProMaster = helper.ApiCall(catalougeURL, EndPoints.SellerProduct + "?brandId=" + id, "GET", null);
                        BaseResponse<SellerProduct> baseSellerProduct = new BaseResponse<SellerProduct>();
                        var sellerProduct = baseSellerProduct.JsonParseList(tempSellerProMaster);
                        sellerProducts = sellerProduct.Data as List<SellerProduct> ?? new List<SellerProduct>();
                    }
                    catch
                    {
                        sellerProducts = new List<SellerProduct>();
                    }

                    if (brandToSeller.Any() || sellerProducts.Any())
                    {
                        if (brandToSeller.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("AssignBrandToSeller", "Brand");
                        }

                        if (sellerProducts.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("SellerProduct", "Brand");
                        }
                    }
                    else
                    {
                        var response = helper.ApiCall(_URL, EndPoints.Brand + "?Id=" + id, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
                else
                {
                    baseResponse = baseResponse.NotExist();
                }
            }
            catch (Exception ex)
            {
                baseResponse = baseResponse.InvalidInput(ex.Message);
            }
            return baseResponse;
        }

        public BaseResponse<BrandLibrary> GetBrand(int? PageIndex, int? PageSize)
        {
            var response = helper.ApiCall(_URL, EndPoints.Brand + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<BrandLibrary> GetBrandById(int id)
        {
            var response = helper.ApiCall(_URL, EndPoints.Brand + "?Id=" + id, "GET", null);
            return baseResponse.JsonParseRecord(response);
        }

        public BaseResponse<BrandLibrary> GetBrandByName(string Name = null)
        {
            var response = helper.ApiCall(_URL, EndPoints.Brand + "?Name=" + Name.Replace("'", "''"), "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<BrandLibrary> GetBrandByStatus(string status, int? PageIndex, int? PageSize)
        {
            var response = helper.ApiCall(_URL, EndPoints.Brand + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + "&status=" + status, "GET", null);
            return baseResponse.JsonParseList(response);
        }


        public BaseResponse<BrandLibrary> Search(string? searchText = null, string? status = null, int? PageIndex = 0, int? PageSize = 0)
        {
            try
            {
                string url = string.Empty;

                if (!string.IsNullOrEmpty(searchText) && searchText != "")
                {
                    url = "&searchText=" + HttpUtility.UrlEncode(searchText);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    url += "&status=" + status;
                }

                var response = helper.ApiCall(_URL, EndPoints.Brand + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
                return baseResponse.JsonParseList(response);
            }
            catch (Exception ex)
            {
                return baseResponse.InvalidInput(ex.Message);
            }
        }

        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {

                var file = FileName;

                var folderName = Path.Combine("Resources", "BrandImages");
                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + a;
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UploadImageAndDocs(fileName, folderName, FileName);
                    return fileName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public string UpdateDocFile(string OldName, string Name, IFormFile? FileName)
        {

            try
            {
                if (FileName != null)
                {
                    var file = FileName;

                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + a;
                    var folderName = Path.Combine("Resources", "BrandImages");
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UpdateUploadImageAndDocs(OldName, fileName, folderName, FileName);

                    return fileName;
                }
                else
                {
                    string fileName = null;
                    if (OldName != null)
                    {
                        fileName = OldName;

                    }
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

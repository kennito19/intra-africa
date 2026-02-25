using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.IdentityModel.Tokens;
using System.Web;

namespace API_Gateway.Common
{
    public class WarehouseDetails
    {
        private readonly HttpContext _httpContext;
        public string _URL = string.Empty;
        public string _cataURL = string.Empty;
        private readonly IConfiguration _configuration;
        BaseResponse<Warehouse> baseResponse = new BaseResponse<Warehouse>();
        private ApiHelper helper;
        public WarehouseDetails(HttpContext httpContext, string URL, string? cataURL = null)
        {
            _httpContext = httpContext;
            _URL = URL;
            _cataURL = cataURL;
            helper = new ApiHelper(_httpContext);
        }

        public BaseResponse<Warehouse> SaveWarehouse(Warehouse model, string UserId)
        {
            var temp = helper.ApiCall(_URL, EndPoints.Warehouse + "?GSTInfoId=" + model.GSTInfoId + "&UserID=" + model.UserID + "&Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<Warehouse> tmp = baseResponse.Data as List<Warehouse>;
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                Warehouse warehouse = new Warehouse();
                warehouse.UserID = model.UserID;
                warehouse.GSTInfoId = model.GSTInfoId;
                warehouse.Name = model.Name;
                warehouse.ContactPersonName = model.ContactPersonName;
                warehouse.ContactPersonMobileNo = model.ContactPersonMobileNo;
                warehouse.AddressLine1 = model.AddressLine1;
                warehouse.AddressLine2 = model.AddressLine2;
                warehouse.Landmark = model.Landmark;
                warehouse.Pincode = model.Pincode;
                warehouse.CityId = model.CityId;
                warehouse.StateId = model.StateId;
                warehouse.CountryId = model.CountryId;
                warehouse.Status = model.Status;
                warehouse.CreatedBy = UserId;
                warehouse.CreatedAt = DateTime.Now;

                var response = helper.ApiCall(_URL, EndPoints.Warehouse, "POST", warehouse);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return baseResponse;
        }

        public BaseResponse<Warehouse> UpdateWarehouse(Warehouse model, string UserId)
        {
            var temp = helper.ApiCall(_URL, EndPoints.Warehouse + "?GSTInfoId=" + model.GSTInfoId + "&UserID=" + model.UserID + "&Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<Warehouse> tmp = baseResponse.Data as List<Warehouse>;
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
                Warehouse warehouse = baseResponse.Data as Warehouse;
                warehouse.UserID = model.UserID;
                warehouse.GSTInfoId = model.GSTInfoId;
                warehouse.Name = model.Name;
                warehouse.ContactPersonName = model.ContactPersonName;
                warehouse.ContactPersonMobileNo = model.ContactPersonMobileNo;
                warehouse.AddressLine1 = model.AddressLine1;
                warehouse.AddressLine2 = model.AddressLine2;
                warehouse.Landmark = model.Landmark;
                warehouse.Pincode = model.Pincode;
                warehouse.CityId = model.CityId;
                warehouse.StateId = model.StateId;
                warehouse.CountryId = model.CountryId;
                warehouse.Status = model.Status;
                warehouse.ModifiedBy = UserId;
                warehouse.ModifiedAt = DateTime.Now;

                response = helper.ApiCall(_URL, EndPoints.Warehouse, "PUT", warehouse);
                if (warehouse.Status.ToLower() != "active")
                {
                    var tempdeleteRecord1 = helper.ApiCall(_cataURL, EndPoints.ProductWarehouse + "/UpdateWarehouseStock" + "?warehouseid=" + warehouse.Id, "PUT", null);
                }

                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return baseResponse;
        }

        public BaseResponse<Warehouse> DeleteWarehouse(int? id = 0, string? userID = "")
        {
            if (id != 0 && !string.IsNullOrEmpty(userID))
            {
                // here we will not passed Id while get the record. (Insted we used userId only, cause we want only user's record.)
                var temp = helper.ApiCall(_URL, EndPoints.Warehouse + "?userID=" + userID, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<Warehouse> tmp = baseResponse.Data as List<Warehouse>;
                if (tmp.Count == 0)
                {
                    baseResponse = baseResponse.NotExist();
                }
                else if (tmp.Count == 1)
                {
                    baseResponse.code = 400;
                    baseResponse.Message = "User should have atleast one wareohuse !!";
                    baseResponse.pagination = null;
                    baseResponse.Data = null;
                }
                else if (tmp.Count > 1)
                {
                    var productWarehouse = helper.ApiCall(_cataURL, EndPoints.ProductWarehouse + "?warehouseid=" + id, "GET", null);
                    BaseResponse<ProductWareHouse> baseProductWarehouse = new BaseResponse<ProductWareHouse>();
                    var warehouseBaseresponse = baseProductWarehouse.JsonParseList(productWarehouse);
                    List<ProductWareHouse> productWareHouses = warehouseBaseresponse.Data as List<ProductWareHouse>;
                    if (productWareHouses.Any())
                    {
                        baseResponse = baseResponse.ChildAlreadyExists("ProductWareHouse", "WareHouse");
                    }
                    else
                    {
                        var tempdeleteRecord = helper.ApiCall(_URL, EndPoints.Warehouse + "?Id=" + id, "DELETE", null);

                        var tempdeleteRecord1 = helper.ApiCall(_cataURL, EndPoints.ProductWarehouse + "/UpdateWarehouseStock" + "?warehouseid=" + id, "PUT", null);

                        baseResponse = baseResponse.JsonParseInputResponse(tempdeleteRecord);
                    }
                }
                else
                {
                    baseResponse.code = 201;
                    baseResponse.Message = "Plesse Enter Id and UserId.";
                    baseResponse.Data = "";
                }
            }
            else
            {
                baseResponse.code = 201;
                baseResponse.Message = "Plesse Enter Id and UserId.";
                baseResponse.Data = "";
            }
            return baseResponse;
        }

        public BaseResponse<Warehouse> DeleteWarehouseWithGst(int? id = 0, int? gstInfoId = 0)
        {
            if (id != 0 && gstInfoId != 0)
            {
                // here we will not passed Id while get the record. (Insted we used userId only, cause we want only user's record.)

                var temp = helper.ApiCall(_URL, EndPoints.Warehouse + "?gstInfoId=" + gstInfoId, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<Warehouse> tmp = baseResponse.Data as List<Warehouse>;
                if (tmp.Count == 0)
                {
                    baseResponse = baseResponse.NotExist();
                }
                else if (tmp.Count == 1)
                {
                    baseResponse.code = 400;
                    baseResponse.Message = "User should have atleast one wareohuse !!";
                    baseResponse.Data = null;
                    baseResponse.pagination = null;
                }
                else if (tmp.Count > 1)
                {
                    var productWarehouse = helper.ApiCall(_cataURL, EndPoints.ProductWarehouse + "?warehouseid=" + id, "GET", null);
                    BaseResponse<ProductWareHouse> baseProductWarehouse = new BaseResponse<ProductWareHouse>();
                    var warehouseBaseresponse = baseProductWarehouse.JsonParseList(productWarehouse);
                    List<ProductWareHouse> productWareHouses = warehouseBaseresponse.Data as List<ProductWareHouse>;
                    if (productWareHouses.Any())
                    {
                        baseResponse = baseResponse.ChildAlreadyExists("ProductWareHouse", "WareHouse");
                    }
                }
                else
                {
                    var tempdeleteRecord = helper.ApiCall(_URL, EndPoints.Warehouse + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(tempdeleteRecord);
                }
            }
            else
            {
                baseResponse.code = 201;
                baseResponse.Message = "Plesse Enter Id and GstInfoId.";
                baseResponse.Data = "";
            }
            return baseResponse;
        }


        public BaseResponse<Warehouse> GetWarehouse(int? PageIndex = 1, int? PageSize = 10)
        {
            ApiHelper helper = new ApiHelper(_httpContext);
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> GetWarehouseById(int id)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?Id=" + id, "GET", null);
            return baseResponse.JsonParseRecord(response);
        }

        public BaseResponse<Warehouse> GetWarehouseByGSTInfoId(int GSTInfoId)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?GSTInfoId=" + GSTInfoId, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> GetWarehouseByStateId(int StateId)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?StateId=" + StateId, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> GetWarehouseByCityId(int CityId)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?CityId=" + CityId, "GET", null);
            return baseResponse.JsonParseList(response);
        }
        public BaseResponse<Warehouse> GetWarehouseByCountryId(int CountryId)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?CountryId=" + CountryId, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> GetWarehouseByUserID(string UserID)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?UserID=" + UserID, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> GetWarehouseByName(string Name)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?Name=" + Name, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> GetWarehouseByContactPersonName(string ContactPersonName)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?ContactPersonName=" + ContactPersonName, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> GetWarehouseByContactPersonMobileNo(string ContactPersonMobileNo)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?ContactPersonMobileNo=" + ContactPersonMobileNo, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> GetWarehouseByPincode(string Pincode)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?Pincode=" + Pincode, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> WarehouseSearch(int? Id, int? GSTInfoId, int? CountryId, int? StateId, int? CityId, string? UserID, string? Name, string? ContactPersonName, string? ContactPersonMobileNo, string? Pincode, string? Status, string? CountryName, string? CityName, string? StateName)
        {
            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?Id=" + Id + "&GSTInfoId=" + GSTInfoId + "&CountryId=" + CountryId + "&StateId=" + StateId + "&CityId=" + CityId + "&UserID=" + UserID + "&Name=" + Name + "&ContactPersonName=" + ContactPersonName + "&ContactPersonMobileNo=" + ContactPersonMobileNo + "&Pincode=" + Pincode + "&Status=" + Status + "&CountryName=" + CountryName + "&StateName=" + StateName + "&CityName=" + CityName, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<Warehouse> Search(int? GSTInfoId = null, string? UserID = null, string? searchtext = null,int? PageIndex = 1, int? PageSize = 10)
        {
            string url = string.Empty;

            if (GSTInfoId!= null && GSTInfoId != 0)
            {
                url = "&GSTInfoId=" + GSTInfoId;
            }

            if (!string.IsNullOrEmpty(UserID) && UserID != "")
            {
                url = "&UserID=" + UserID;
            }

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(_URL, EndPoints.Warehouse + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            return baseResponse.JsonParseList(response);
        }


        public BaseResponse<Warehouse> DeleteUserWarehouse(int? id = 0)
        {
            if (id != 0)
            {
                // here we will not passed Id while get the record. (Insted we used userId only, cause we want only user's record.)
                var temp = helper.ApiCall(_URL, EndPoints.Warehouse + "?id=" + id, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<Warehouse> tmp = baseResponse.Data as List<Warehouse>;
                if (tmp.Count > 0)
                {
                    var tempdeleteRecord = helper.ApiCall(_URL, EndPoints.Warehouse + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(tempdeleteRecord);
                }
                else
                {
                    baseResponse = baseResponse.NotExist();
                   
                }
            }
            else
            {
                baseResponse.code = 201;
                baseResponse.Message = "Plesse Enter Id.";
                baseResponse.Data = "";
            }
            return baseResponse;
        }
    }
}

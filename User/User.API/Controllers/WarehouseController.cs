using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Domain.Entity;
using User.Domain;
using Microsoft.AspNetCore.Authorization;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(Warehouse warehouse)
        {
            warehouse.CreatedAt = DateTime.Now;
            var data = await _warehouseService.Create(warehouse);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(Warehouse warehouse)
        {
            warehouse.ModifiedAt = DateTime.Now;
            var data = await _warehouseService.Update(warehouse);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id, int? gstInfoId, string? userID)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            Warehouse warehouse = new Warehouse();
            warehouse.Id = id;
            warehouse.GSTInfoId = gstInfoId;
            warehouse.UserID = userID;
            warehouse.DeletedBy = DeletedBy;
            warehouse.DeletedAt = DateTime.Now;

            var data = await _warehouseService.Delete(warehouse);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<Warehouse>>> Get(int? Id = null, string? UserID = null, int? GSTInfoId = null, string? GSTNo = null, string? Name = null,string? ContactPersonName = null,string? ContactPersonMobileNo = null,string? Pincode = null, int? CityId = null, int? StateId = null, int? CountryId = null, string? Status = null, bool? IsDeleted = false, string? CountryName = null, string? CityName = null, string? StateName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get",string? searchText=null)
        {
            Warehouse warehouse = new Warehouse();
            if (Id != null)
            {
                warehouse.Id = Convert.ToInt32(Id);
            }
            if (GSTInfoId != null)
            {
                warehouse.GSTInfoId = Convert.ToInt32(GSTInfoId);
            }if (CountryId != null)
            {
                warehouse.CountryId = Convert.ToInt32(CountryId);
            }
            if (StateId != null)
            {
                warehouse.StateId = Convert.ToInt32(StateId);
            }
            if (CityId != null)
            {
                warehouse.CityId = Convert.ToInt32(CityId);
            }
            warehouse.UserID = UserID;
            warehouse.GSTNo = GSTNo;
            warehouse.Name = Name;
            warehouse.ContactPersonName = ContactPersonName;
            warehouse.ContactPersonMobileNo = ContactPersonMobileNo;
            warehouse.Pincode = Pincode;
            warehouse.searchText = searchText;
            warehouse.Status = Status;
            warehouse.IsDeleted = Convert.ToBoolean(IsDeleted);
            warehouse.CountryName = CountryName;
            warehouse.CityName = CityName;
            warehouse.StateName = StateName;
            var data = await _warehouseService.Get(warehouse, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

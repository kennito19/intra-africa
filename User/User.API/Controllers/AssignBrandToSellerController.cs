using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Application.Services;
using User.Domain.Entity;
using User.Domain;
using Microsoft.AspNetCore.Authorization;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AssignBrandToSellerController : ControllerBase
    {
        private readonly IAssignBrandToSellerService _assignBrandToSellerService;

        public AssignBrandToSellerController(IAssignBrandToSellerService assignBrandToSellerService)
        {
            _assignBrandToSellerService = assignBrandToSellerService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(AssignBrandToSeller assignBrandToSeller)
        {
            assignBrandToSeller.CreatedAt = DateTime.Now;
            var data = await _assignBrandToSellerService.Create(assignBrandToSeller);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(AssignBrandToSeller assignBrandToSeller)
        {
            assignBrandToSeller.ModifiedAt = DateTime.Now;
            var data = await _assignBrandToSellerService.Update(assignBrandToSeller);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id)
        {

            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            AssignBrandToSeller assignBrandToSeller = new AssignBrandToSeller();
            assignBrandToSeller.Id = id;
            assignBrandToSeller.DeletedBy = DeletedBy;
            assignBrandToSeller.DeletedAt = DateTime.Now;


            var data = await _assignBrandToSellerService.Delete(assignBrandToSeller);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<AssignBrandToSeller>>> Get(int? id = null, string? SellerID = null, int BrandId = 0, string? brandname = null, string? status = null, string? brandStatus = null, bool? isDeleted = false, bool? isBrandDeleted = false, int pageIndex = 1, int pageSize = 10, string? mode = "get", string? searchtext = null)
        {
            AssignBrandToSeller assignBrandToSeller = new AssignBrandToSeller();
            if (id != null)
            {
                assignBrandToSeller.Id = Convert.ToInt32(id);
            }

            assignBrandToSeller.SellerID = SellerID;
            assignBrandToSeller.BrandId = BrandId;
            assignBrandToSeller.BrandName = brandname;
            assignBrandToSeller.Searchtext = searchtext;
            assignBrandToSeller.Status = status;
            assignBrandToSeller.BrandStatus = brandStatus;
            assignBrandToSeller.IsDeleted = Convert.ToBoolean(isDeleted);
            assignBrandToSeller.IsBrandDeleted = Convert.ToBoolean(isBrandDeleted);

            var data = await _assignBrandToSellerService.Get(assignBrandToSeller, pageIndex, pageSize, mode);
            return data;
        }


    }
}

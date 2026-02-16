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
    
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(Brand brand)
        {
            brand.CreatedAt = DateTime.Now;
            var data = await _brandService.Create(brand);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(Brand brand)
        {
            brand.ModifiedAt = DateTime.Now;
            var data = await _brandService.Update(brand);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id)
        {

            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            Brand brand = new Brand();
            brand.ID = id;
            brand.DeletedBy = DeletedBy;
            brand.DeletedAt = DateTime.Now;


            var data = await _brandService.Delete(brand);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<Brand>>> Get(int? id = null, string? brandIds = null, string? name = null, string? status = null, bool? isDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText=null)
        {
            Brand brand = new Brand();
            if (id != null)
            {
                brand.ID = Convert.ToInt32(id);
            }
            
            brand.BrandIds = brandIds;
            brand.Name = name;
            brand.Status = status;
            brand.searchText = searchText;
            brand.IsDeleted = Convert.ToBoolean(isDeleted);
             
            var data = await _brandService.Get(brand, PageIndex, PageSize, Mode);
            return data;
        }

    }
}

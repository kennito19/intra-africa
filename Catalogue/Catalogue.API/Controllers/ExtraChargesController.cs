using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ExtraChargesController : ControllerBase
    {
        private readonly IExtraChargesService _extraChargesService;

        public ExtraChargesController(IExtraChargesService extraChargesService)
        {
            _extraChargesService = extraChargesService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            extraChargesLibrary.CreatedAt = DateTime.Now;
            var data = await _extraChargesService.AddExtraCharges(extraChargesLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            extraChargesLibrary.ModifiedAt = DateTime.Now;
            var data = await _extraChargesService.UpdateExtraCharges(extraChargesLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteExtraCharges(int id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ExtraChargesLibrary extraChargesLibrary = new ExtraChargesLibrary();
            extraChargesLibrary.Id = id;
            extraChargesLibrary.DeletedBy = DeletedBy;
            extraChargesLibrary.DeletedAt = DateTime.Now;
            var data = await _extraChargesService.DeleteExtraCharges(extraChargesLibrary);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ExtraChargesLibrary>>> getExtraCharges(int? id = null, int? catId = null, string? name = null, string? categoryName = null, string? chargespaidbyname = null, bool? isDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get",string? searchText=null)
        {
            ExtraChargesLibrary extraChargesLibrary = new ExtraChargesLibrary();
            if (id != null)
            {
                extraChargesLibrary.Id = Convert.ToInt32(id);
            }
            extraChargesLibrary.CatID = catId;
            extraChargesLibrary.Name = name;
            extraChargesLibrary.CategoryName = categoryName;
            extraChargesLibrary.ChargesPaidByName = chargespaidbyname;
            extraChargesLibrary.searchText = searchText;
            extraChargesLibrary.IsDeleted = Convert.ToBoolean(isDeleted);
            //if (isCompulsary != null)
            //{
            //    extraChargesLibrary.IsCompulsary = Convert.ToBoolean(isCompulsary);
            //}


            var data = await _extraChargesService.GetExtraCharges(extraChargesLibrary, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("CatExtraCharges")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ExtraChargesLibrary>>> getCatExtraCharges(int CategoryId)
        {
            var data = await _extraChargesService.GetCatExtraCharges(CategoryId);
            return data;
        }
    }
}

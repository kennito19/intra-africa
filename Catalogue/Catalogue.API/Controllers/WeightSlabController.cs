using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class WeightSlabController : ControllerBase
    {
        private readonly IWeightSlabService _weightSlabService;

        public WeightSlabController(IWeightSlabService weightSlabService)
        {
            _weightSlabService = weightSlabService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(WeightSlabLibrary weightSlab)
        {
            weightSlab.CreatedAt = DateTime.Now;
            weightSlab.LocalCharges = weightSlab.LocalCharges;
            weightSlab.ZonalCharges = weightSlab.ZonalCharges;
            weightSlab.NationalCharges = weightSlab.NationalCharges;
            var data = await _weightSlabService.Create(weightSlab);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(WeightSlabLibrary weightSlab)
        {
            weightSlab.ModifiedAt = DateTime.Now;
            weightSlab.LocalCharges = weightSlab.LocalCharges;
            weightSlab.ZonalCharges = weightSlab.ZonalCharges;
            weightSlab.NationalCharges = weightSlab.NationalCharges;
            var data = await _weightSlabService.Update(weightSlab);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            WeightSlabLibrary weightSlab = new WeightSlabLibrary();
            weightSlab.Id = Id;
            weightSlab.DeletedBy = DeletedBy;
            weightSlab.DeletedAt = DateTime.Now;
            var data = await _weightSlabService.Delete(weightSlab);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<WeightSlabLibrary>>> get(int Id = 0, string? WeightSlab = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText = null)
        {
            WeightSlabLibrary weightSlabs = new WeightSlabLibrary();
            weightSlabs.Id = Id;
            weightSlabs.WeightSlab = WeightSlab;
            weightSlabs.Searchtext = searchText;
            weightSlabs.IsDeleted = Isdeleted;

            var data = await _weightSlabService.get(weightSlabs, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

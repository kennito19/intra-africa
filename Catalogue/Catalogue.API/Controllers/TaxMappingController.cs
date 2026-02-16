using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxMappingController : ControllerBase
    {
        private readonly ITaxMappingService _taxMapService;

        public TaxMappingController(ITaxMappingService taxMapService)
        {
            _taxMapService = taxMapService;
        }
        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddTaxMapping(TaxMapping taxMapping)
        {
            taxMapping.CreatedAt = DateTime.Now;
            var data = await _taxMapService.AddTaxMapping(taxMapping);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateTaxMapping(TaxMapping taxMapping)
        {
            taxMapping.ModifiedAt = DateTime.Now;
            var data = await _taxMapService.UpdateTaxMapping(taxMapping);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteTaxMapping(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            TaxMapping taxMapping = new TaxMapping();
            taxMapping.Id = Id;
            taxMapping.DeletedBy = DeletedBy;
            taxMapping.DeletedAt = DateTime.Now;
            var data = await _taxMapService.DeleteTaxMapping(taxMapping);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<TaxMapping>>> GetTaxMapping(int? id = null, string? tax = null, string? taxMapBy = null, string? taxType = null, int? taxId = 0, int? taxTypeId = 0, int? taxValueId = 0, bool? isDeleted = false, bool Getparent = false, bool Getchild = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            TaxMapping taxMapping = new TaxMapping();
            if (id != null)
            {
                taxMapping.Id = Convert.ToInt32(id);
            }

            if (taxId != null)
            {
                taxMapping.TaxId = Convert.ToInt32(taxId);
            }

            if (taxTypeId != null)
            {
                taxMapping.TaxTypeId = Convert.ToInt32(taxTypeId);
            }

            if (taxValueId != null)
            {
                taxMapping.TaxValueId = Convert.ToInt32(taxValueId);
            }

            taxMapping.Tax = tax;
            taxMapping.TaxType = taxType;
            taxMapping.TaxMapBy = taxMapBy;
            taxMapping.IsDeleted = Convert.ToBoolean(isDeleted);
            taxMapping.Searchtext = Searchtext;
            var data = await _taxMapService.GetTaxMapping(taxMapping, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

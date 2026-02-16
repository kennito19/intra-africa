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
    
    public class TaxTypeValueController : ControllerBase
    {
        private readonly ITaxTypeValueService _taxTypeValueService;

        public TaxTypeValueController(ITaxTypeValueService taxTypeValueService)
        {
            _taxTypeValueService = taxTypeValueService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            taxTypeValueLibrary.CreatedAt = DateTime.Now;
            var data = await _taxTypeValueService.AddTaxTypeValue(taxTypeValueLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            taxTypeValueLibrary.ModifiedAt = DateTime.Now;
            var data = await _taxTypeValueService.UpdateTaxTypeValue(taxTypeValueLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteTaxType(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            TaxTypeValueLibrary taxTypeValueLibrary = new TaxTypeValueLibrary();
            taxTypeValueLibrary.Id = Id;
            taxTypeValueLibrary.DeletedBy = DeletedBy;
            taxTypeValueLibrary.DeletedAt = DateTime.Now;
            var data = await _taxTypeValueService.DeleteTaxTypeValue(taxTypeValueLibrary);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<TaxTypeValueLibrary>>> getTaxTypeValue(int? id = null, int? taxTypeId = null, string? name = null, bool? isDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            TaxTypeValueLibrary taxTypeValueLibrary = new TaxTypeValueLibrary();
            if (id != null)
            {
                taxTypeValueLibrary.Id = Convert.ToInt32(id);
            }
            taxTypeValueLibrary.TaxTypeID = Convert.ToInt32(taxTypeId);
            taxTypeValueLibrary.Name = name;
            taxTypeValueLibrary.IsDeleted = Convert.ToBoolean(isDeleted);
            taxTypeValueLibrary.Searchtext = Searchtext;
            var data = await _taxTypeValueService.GetTaxTypeValue(taxTypeValueLibrary, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

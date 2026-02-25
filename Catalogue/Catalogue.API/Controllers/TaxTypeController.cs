using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Catalogue.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TaxTypeController : ControllerBase
    {
        private readonly ITaxTypeService _taxTypeService;

        public TaxTypeController(ITaxTypeService taxTypeService)
        {
            _taxTypeService = taxTypeService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            taxTypeLibrary.CreatedAt = DateTime.Now;
            var data = await _taxTypeService.AddTaxType(taxTypeLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            taxTypeLibrary.ModifiedAt = DateTime.Now;
            var data = await _taxTypeService.UpdateTaxType(taxTypeLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteTaxType(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            TaxTypeLibrary taxTypeLibrary = new TaxTypeLibrary();
            taxTypeLibrary.Id = Id;
            taxTypeLibrary.DeletedBy = DeletedBy;
            taxTypeLibrary.DeletedAt = DateTime.Now;
            var data = await _taxTypeService.DeleteTaxType(taxTypeLibrary);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<TaxTypeLibrary>>> getTaxType(int? id = null, string? taxType = null, int? ParentID = null, bool? isDeleted = false, bool Getparent = false, bool Getchild = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            TaxTypeLibrary taxTypeLibrary = new TaxTypeLibrary();
            if (id != null)
            {
                taxTypeLibrary.Id = Convert.ToInt32(id);
            }
            taxTypeLibrary.TaxType = taxType;
            taxTypeLibrary.ParentId = (ParentID.HasValue && ParentID.Value > 0) ? ParentID : null;
            taxTypeLibrary.IsDeleted = Convert.ToBoolean(isDeleted);
            taxTypeLibrary.Searchtext = Searchtext;
            var data = await _taxTypeService.GetTaxType(taxTypeLibrary, Getparent, Getchild, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

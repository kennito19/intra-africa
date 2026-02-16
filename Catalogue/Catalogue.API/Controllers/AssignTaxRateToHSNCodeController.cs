using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Xml.Linq;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignTaxRateToHSNCodeController : ControllerBase
    {
        private readonly ITaxRateToHSNCodeService _taxRateToHsnCodeService;


        public AssignTaxRateToHSNCodeController(ITaxRateToHSNCodeService taxRateToHsnCodeService)
        {
            _taxRateToHsnCodeService = taxRateToHsnCodeService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            rateToHSNCode.CreatedAt = DateTime.Now;
            var data = await _taxRateToHsnCodeService.Create(rateToHSNCode);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            rateToHSNCode.ModifiedAt = DateTime.Now;
            var data = await _taxRateToHsnCodeService.Update(rateToHSNCode);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            AssignTaxRateToHSNCodeLibrary rateToHSNCode = new AssignTaxRateToHSNCodeLibrary();
            rateToHSNCode.Id = Id;
            var data = await _taxRateToHsnCodeService.Delete(rateToHSNCode);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<AssignTaxRateToHSNCodeLibrary>>> get(int Id = 0, int HsnCodeId = 0, int TaxValueId = 0, string? HsnCode = null, string? TaxName = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            AssignTaxRateToHSNCodeLibrary rateToHSNCode = new AssignTaxRateToHSNCodeLibrary();
            rateToHSNCode.Id = Id;
            rateToHSNCode.HsnCodeId = HsnCodeId;
            rateToHSNCode.TaxValueId = TaxValueId;
            rateToHSNCode.HsnCode = HsnCode;
            rateToHSNCode.TaxName = TaxName;
            rateToHSNCode.SearchText = Searchtext;

            var data = await _taxRateToHsnCodeService.get(rateToHSNCode, PageIndex, PageSize, Mode);
            return data;
        }


    }
}

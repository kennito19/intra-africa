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
    
    public class HSNCodeController : ControllerBase
    {
        private readonly IHSNCodeService _hSNCodeService;

        public HSNCodeController(IHSNCodeService hSNCodeService)
        {
            _hSNCodeService = hSNCodeService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            hSNCodeLibrary.CreatedAt = DateTime.Now;

            var data = await _hSNCodeService.addHSNCode(hSNCodeLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            hSNCodeLibrary.ModifiedAt = DateTime.Now;
            var data = await _hSNCodeService.updateHSNCode(hSNCodeLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteHSNCode(int id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            HSNCodeLibrary hSNCodeLibrary = new HSNCodeLibrary();
            hSNCodeLibrary.Id = id;
            hSNCodeLibrary.DeletedBy = DeletedBy;
            hSNCodeLibrary.DeletedAt = DateTime.Now;
            var data = await _hSNCodeService.deleteHSNCode(hSNCodeLibrary);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<HSNCodeLibrary>>> getHSNCode(int? id = null, string? hsncode = null, bool? isDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            HSNCodeLibrary hSNCodeLibrary = new HSNCodeLibrary();
            if (id != null)
            {
                hSNCodeLibrary.Id = Convert.ToInt32(id);
            }
            hSNCodeLibrary.HSNCode = hsncode;
            hSNCodeLibrary.IsDeleted = Convert.ToBoolean(isDeleted);
            hSNCodeLibrary.Searchtext = Searchtext;
            var data = await _hSNCodeService.getHSNCode(hSNCodeLibrary, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

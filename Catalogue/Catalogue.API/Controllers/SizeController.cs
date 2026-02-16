using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddSize(SizeLibrary sizeLibrary)
        {
            sizeLibrary.CreatedAt = DateTime.Now;


            var data = await _sizeService.Create(sizeLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateSize(SizeLibrary sizeLibrary)
        {
            sizeLibrary.ModifiedAt = DateTime.Now;

            var data = await _sizeService.Update(sizeLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteSize(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SizeLibrary sizeLibrary = new SizeLibrary();
            sizeLibrary.Id = Id;
            sizeLibrary.DeletedBy = DeletedBy;
            sizeLibrary.DeletedAt = DateTime.Now;
            var data = await _sizeService.Delete(sizeLibrary);
            return data;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<SizeLibrary>>> GetSize(int? Id = 0, string? TypeName = null, string? status = null, bool? Isdeleted = false, int? parentId = 0, bool? Getparent = false, bool? Getchild = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            SizeLibrary size = new SizeLibrary();
            size.Id = Convert.ToInt32( Id);
            size.TypeName = TypeName;
            size.Status = status;
            size.IsDeleted = Convert.ToBoolean( Isdeleted);
            size.ParentId = parentId;
            size.Searchtext = Searchtext;
            

            var data = await _sizeService.Get(size, Convert.ToBoolean(Getparent), Convert.ToBoolean(Getchild), PageIndex, PageSize, Mode);
            return data;
        }
    }
}

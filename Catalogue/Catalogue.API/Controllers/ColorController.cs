using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ColorLibrary color)
        {
            color.CreatedAt = DateTime.Now;

            var data = await _colorService.Create(color);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ColorLibrary color)
        {
            color.ModifiedAt = DateTime.Now;
            var data = await _colorService.Update(color);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ColorLibrary color = new ColorLibrary();
            color.Id = Id;
            color.DeletedAt = DateTime.Now;
            color.DeletedBy = DeletedBy;
            var data = await _colorService.Delete(color);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]

        public async Task<BaseResponse<List<ColorLibrary>>> get(int Id = 0, string? Name = null, string? Code = null, string? Guid = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ColorLibrary color = new ColorLibrary();
            color.Id = Id;
            color.Name = Name;
            color.Code = HttpUtility.UrlDecode(Code);
            color.Guid = Guid;
            color.IsDeleted = Isdeleted;
            color.Searchtext = HttpUtility.UrlDecode(Searchtext);

            var data = await _colorService.get(color, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class SpecificationController : ControllerBase
    {
        private readonly ISpecificationService _specificationService;

        public SpecificationController(ISpecificationService specificationService)
        {
            _specificationService = specificationService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(SpecificationLibrary specification)
        {
            specification.CreatedAt = DateTime.Now;
            var data = await _specificationService.Create(specification);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(SpecificationLibrary specification)
        {
            specification.ModifiedAt = DateTime.Now;
            var data = await _specificationService.Update(specification);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SpecificationLibrary specification = new SpecificationLibrary();
            specification.ID = Id;
            specification.DeletedBy = DeletedBy;
            specification.DeletedAt = DateTime.Now;
            var data = await _specificationService.Delete(specification);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<SpecificationLibrary>>> get(int Id = 0, int? ParentID = 0, string? Name = null, string? PathIds = null, string? FieldType = null, string? Guid = null, bool Isdeleted = false,string? Searchtext=null, bool IsChildParent = false, bool Getparent = false, bool Getchild = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            SpecificationLibrary specification = new SpecificationLibrary();
            specification.ID = Id;
            specification.ParentId = ParentID;
            specification.Name = Name;
            specification.PathIds = PathIds;
            specification.FieldType = FieldType;
            
            //if(Guid!= null)
            //{
            //    specification.Guid = new Guid(Guid);
            //}
            specification.Guid = Guid;
            specification.isDeleted = Isdeleted;
            specification.IsChildParent = IsChildParent;
            specification.Searchtext = Searchtext;

            var data = await _specificationService.get(specification, Getparent, Getchild, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

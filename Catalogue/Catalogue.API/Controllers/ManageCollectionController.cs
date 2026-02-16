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
    
    public class ManageCollectionController : ControllerBase
    {
        private readonly IManageCollectionService _collectionService;

        public ManageCollectionController(IManageCollectionService collection)
        {
            _collectionService = collection;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageCollectionLibrary collection)
        {
            collection.CreatedAt = DateTime.Now;
            var data = await _collectionService.Create(collection);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageCollectionLibrary collection)
        {
            collection.ModifiedAt = DateTime.Now;
            var data = await _collectionService.Update(collection);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ManageCollectionLibrary collection = new ManageCollectionLibrary();
            collection.Id = Id;
            collection.DeletedBy = DeletedBy;
            collection.DeletedAt = DateTime.Now;
            var data = await _collectionService.Delete(collection);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageCollectionLibrary>>> get(int Id = 0, string? Name = null, bool date = false, string? Type = null, string? Title = null, string? Status = null, bool IsDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageCollectionLibrary collection = new ManageCollectionLibrary();
            collection.Id = Id;
            collection.Name = Name;
            collection.Type = Type;
            collection.Title = Title;
            collection.Status = Status;
            collection.IsDeleted = IsDeleted;
            collection.SearchText = Searchtext;
            if (date)
            {
                collection.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            var data = await _collectionService.get(collection, PageIndex, PageSize, Mode);
            return data;
        }


    }
}

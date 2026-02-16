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
    
    public class ManageCollectionMappingController : ControllerBase
    {
        private readonly IManageCollectionMappingService _collectionMappingService;

        public ManageCollectionMappingController(IManageCollectionMappingService collectionMapping)
        {
            _collectionMappingService = collectionMapping;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ManageCollectionMappingLibrary collectionMapping)
        {
            collectionMapping.CreatedAt = DateTime.Now;
            var data = await _collectionMappingService.Create(collectionMapping);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ManageCollectionMappingLibrary collectionMapping)
        {
            collectionMapping.ModifiedAt = DateTime.Now;
            var data = await _collectionMappingService.Update(collectionMapping);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ManageCollectionMappingLibrary collectionMapping = new ManageCollectionMappingLibrary();
            collectionMapping.Id = Id;
            collectionMapping.DeletedBy = DeletedBy;
            collectionMapping.DeletedAt = DateTime.Now;
            var data = await _collectionMappingService.Delete(collectionMapping);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ManageCollectionMappingLibrary>>> get(int Id = 0, int ProductId = 0, int CollectionId = 0, string? SellerId = null, string? CollectionName = null, string? ProductName = null, bool IsDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ManageCollectionMappingLibrary collectionMapping = new ManageCollectionMappingLibrary();
            collectionMapping.Id = Id;
            collectionMapping.CollectionId = CollectionId;
            collectionMapping.ProductId = ProductId;
            collectionMapping.SellerId = SellerId;
            collectionMapping.CollectionName = CollectionName;
            collectionMapping.ProductName = ProductName;
            collectionMapping.IsDeleted = IsDeleted;
            collectionMapping.SearchText = Searchtext;

            var data = await _collectionMappingService.get(collectionMapping, PageIndex, PageSize, Mode);
            return data;
        }


    }
}

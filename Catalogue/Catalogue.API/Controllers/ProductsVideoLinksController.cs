using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    
    //public class ProductsVideoLinksController : ControllerBase
    //{
    //    private readonly IProductsVideoLinksService _videoLinksService;
    //    public ProductsVideoLinksController(IProductsVideoLinksService videoLinksService)
    //    {
    //        _videoLinksService = videoLinksService;
    //    }

    //    [HttpPost]
    //    [Route("")]
    //    [Authorize(Policy = "Accessable")]
    //    public async Task<BaseResponse<long>> Create(ProductVideoLinks videoLinks)
    //    {
    //        videoLinks.CreatedAt = DateTime.Now;
    //        var data = await _videoLinksService.Create(videoLinks);
    //        return data;
    //    }

    //    [HttpDelete]
    //    [Route("")]
    //    [Authorize(Policy = "Accessable")]
    //    public async Task<BaseResponse<long>> Delete(int ProductID)
    //    {
    //        ProductVideoLinks videoLinks = new ProductVideoLinks();
    //        videoLinks.ProductID = ProductID;
    //        var data = await _videoLinksService.Delete(videoLinks);
    //        return data;
    //    }

    //    [HttpGet]
    //    [Authorize(Policy = "General")]
    //    public async Task<BaseResponse<List<ProductVideoLinks>>> get(int ID = 0, int? ProductID = 0, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
    //    {
    //        ProductVideoLinks videoLinks = new ProductVideoLinks();
    //        videoLinks.Id = ID;
    //        videoLinks.ProductID = ProductID;

    //        var data = await _videoLinksService.get(videoLinks, PageIndex, PageSize, Mode);
    //        return data;
    //    }
    //}
}

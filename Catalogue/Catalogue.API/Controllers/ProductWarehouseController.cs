using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Catalogue.Domain.DTO;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductWarehouseController : ControllerBase
    {
        private readonly IProductWarehouseService _productWarehouseService;

        public ProductWarehouseController(IProductWarehouseService productWarehouseService)
        {
            _productWarehouseService = productWarehouseService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> AddProductWarehouse(ProductWarehouse productWarehouse)
        {
            productWarehouse.CreatedAt = DateTime.Now;
            var data = await _productWarehouseService.AddProductWarehouse(productWarehouse);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateProductWarehouse(ProductWarehouse productWarehouse)
        {
            productWarehouse.ModifiedAt = DateTime.Now;
            var data = await _productWarehouseService.UpdateProductWarehouse(productWarehouse);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> DeleteProductWarehouse(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ProductWarehouse productWarehouse = new ProductWarehouse();
            productWarehouse.Id = Id;
            productWarehouse.DeletedBy = DeletedBy;
            productWarehouse.DeletedAt = DateTime.Now;
            var data = await _productWarehouseService.DeleteProductWarehouse(productWarehouse);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductWarehouse>>> getProductWarehouse(int? id = null, int? sellerproductid = null, int? sellerwiseproductpricemasterid = null, int? productId = null, int? warehouseid = null, string? warehousename = null, bool? isDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText = null)
        {
            ProductWarehouse productWarehouse = new ProductWarehouse();
            if (id != null)
            {
                productWarehouse.Id = Convert.ToInt32(id);
            }
            if (productId != null)
            {
                productWarehouse.ProductID = Convert.ToInt32(productId);
            }
            if (sellerproductid != null)
            {
                productWarehouse.SellerProductID = Convert.ToInt32(sellerproductid);
            }
            if (sellerwiseproductpricemasterid != null)
            {
                productWarehouse.SellerWiseProductPriceMasterID = Convert.ToInt32(sellerwiseproductpricemasterid);
            }
            if (warehouseid != null)
            {
                productWarehouse.WarehouseID = Convert.ToInt32(warehouseid);
            }
            productWarehouse.WarehouseName = warehousename;
            productWarehouse.IsDeleted = Convert.ToBoolean(isDeleted);
            var data = await _productWarehouseService.GetProductWarehouse(productWarehouse, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("GetSizeWiseWarehouse")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<SizeWiseWarehouse>>> GetSizeWiseWarehouse(int sellerproductid, int productId, int sizeId)
        {
            SizeWiseWarehouse productWarehouse = new SizeWiseWarehouse();
            if (productId != null)
            {
                productWarehouse.ProductId = Convert.ToInt32(productId);
            }
            if (sellerproductid != null)
            {
                productWarehouse.SellerProductId = Convert.ToInt32(sellerproductid);
            }
            if (sizeId != null)
            {
                productWarehouse.SizeId = Convert.ToInt32(sizeId);
            }
            var data = await _productWarehouseService.GetSizeWiseWarehouse(productWarehouse);
            return data;
        }

        [HttpPut]
        [Route("UpdateWarehouseStock")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> UpdateWarehouseStock(int warehouseid)
        {
            var data = await _productWarehouseService.UpdateWarehouseStock(warehouseid);
            return data;
        }
    }
}

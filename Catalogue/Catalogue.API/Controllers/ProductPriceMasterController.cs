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
    
    public class ProductPriceMasterController : ControllerBase
    {
        private readonly IProductPriceMasterService _productPriceMaster;
        public ProductPriceMasterController(IProductPriceMasterService productPriceMaster)
        {
            _productPriceMaster = productPriceMaster;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ProductPriceMaster productPriceMaster)
        {
            productPriceMaster.CreatedAt = DateTime.Now;
            productPriceMaster.MarginIn = productPriceMaster.MarginIn;
            productPriceMaster.MarginCost = productPriceMaster.MarginCost;
            productPriceMaster.MarginPercentage = productPriceMaster.MarginPercentage;
            var data = await _productPriceMaster.Create(productPriceMaster);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ProductPriceMaster productPriceMaster)
        {
            productPriceMaster.ModifiedAt = DateTime.Now;
            productPriceMaster.MarginIn = productPriceMaster.MarginIn;
            productPriceMaster.MarginCost = productPriceMaster.MarginCost;
            productPriceMaster.MarginPercentage = productPriceMaster.MarginPercentage;
            var data = await _productPriceMaster.Update(productPriceMaster);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int? SellerProductID = 0, int? id = 0)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ProductPriceMaster productPriceMaster = new ProductPriceMaster();
            if (SellerProductID > 0)
            {
                productPriceMaster.SellerProductID = SellerProductID;
            }
            if (id > 0)
            {
                productPriceMaster.Id = Convert.ToInt32(id);
            }
            productPriceMaster.DeletedBy = DeletedBy;
            productPriceMaster.DeletedAt = DateTime.Now;
            var data = await _productPriceMaster.Delete(productPriceMaster);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductPriceMaster>>> Get(int ID = 0, int? ProductID = 0, int? SellerProductID = 0, string? SizeName = null, int? SizeID = 0, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            ProductPriceMaster productPriceMaster = new ProductPriceMaster();
            productPriceMaster.Id = ID;
            productPriceMaster.ProductID = ProductID;
            productPriceMaster.SellerProductID = SellerProductID;
            productPriceMaster.SizeID = SizeID;
            productPriceMaster.IsDeleted = Isdeleted;
            productPriceMaster.SizeName = SizeName;


            var data = await _productPriceMaster.Get(productPriceMaster, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

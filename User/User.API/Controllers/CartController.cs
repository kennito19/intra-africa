using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Application.Services;
using User.Domain.Entity;
using User.Domain;
using Microsoft.AspNetCore.Authorization;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "General")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<long>> Create(Cart cart)
        {
            cart.CreatedAt = DateTime.Now;
            var data = await _cartService.Create(cart);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<long>> Update(Cart cart)
        {
            cart.ModifiedAt = DateTime.Now;
            var data = await _cartService.Update(cart);
            return data;
        }

        [HttpPut]
        [Route("AddUserIdinCart")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<long>> AddUserIdinCart(AddCartUserId cart)
        {
            var data = await _cartService.AddUserIdinCart(cart);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<long>> Delete(string sessionId, int? id = null, string? userId = null, int? sellerProductMasterId=null, string? sellerProductIds = null)
        {
            Cart cart = new Cart();
            cart.SessionId = sessionId;
            if(sellerProductMasterId!=null && sellerProductMasterId != 0)
            {
                cart.SellerProductMasterId = Convert.ToInt32(sellerProductMasterId);
            }
            if (id != null && id != 0)
            {
                cart.Id = Convert.ToInt32(id);
            }
            if (!string.IsNullOrEmpty(userId))
            {
                cart.UserId = userId;
            }

            var data = await _cartService.Delete(cart, sellerProductIds);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<Cart>>> Get(int? id = null, string? userId = null, int? sizeId = null, string? sessionId = null, int? sellerProductMasterId = null,int? WarrantyId = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText = null, string? sellerProductIds = null)
        {
            Cart cart = new Cart();
            if (id != null)
            {
                cart.Id = Convert.ToInt32(id);
            }
            if (userId != null)
            {
                cart.UserId = Convert.ToString(userId);
            }
            if (sellerProductMasterId != null)
            {
                cart.SellerProductMasterId = Convert.ToInt32(sellerProductMasterId);
            }
            if (sizeId != null)
            {
                cart.SizeId = Convert.ToInt32(sizeId);
            }
            if (WarrantyId != null)
            {
                cart.WarrantyId = Convert.ToInt32(WarrantyId);
            }
            cart.SessionId = sessionId;
            cart.searchText = searchText;
            var data = await _cartService.Get(cart, PageIndex, PageSize, Mode, sellerProductIds);
            return data;
        }


        [HttpGet]
        [Route("AbandonedCart")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<Cart>>> GetAbandonedCart(int? id = null, string? userId = null,  int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? searchText = null, string? sellerProductIds = null)
        {
            Cart cart = new Cart();
            if (id != null)
            {
                cart.Id = Convert.ToInt32(id);
            }
            if (userId != null)
            {
                cart.UserId = Convert.ToString(userId);
            }
            cart.searchText = searchText;
            var data = await _cartService.Get(cart, PageIndex, PageSize, Mode, sellerProductIds);
            return data;
        }
    }
}


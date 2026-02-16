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
    
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(Wishlist wishlist)
        {
            wishlist.CreatedAt = DateTime.Now;
            var data = await _wishlistService.Create(wishlist);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(Wishlist wishlist)
        {
            wishlist.ModifiedAt = DateTime.Now;
            var data = await _wishlistService.Update(wishlist);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(string userId, string productId)
        {
            Wishlist wishlist = new Wishlist();
            wishlist.UserId = userId;
            wishlist.ProductId = productId;


            var data = await _wishlistService.Delete(wishlist);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<Wishlist>>> Get(int? id = null, string? userId = null, string? productId = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            Wishlist wishlist = new Wishlist();
            if (id != null)
            {
                wishlist.Id = Convert.ToInt32(id);
            }
            if (userId != null)
            {
                wishlist.UserId = Convert.ToString(userId);
            }
            if (productId != null)
            {
                wishlist.ProductId = Convert.ToString(productId);
            }
            var data = await _wishlistService.Get(wishlist, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

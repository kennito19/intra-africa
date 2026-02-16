using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Application.IServices;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public Task<BaseResponse<long>> Create(Wishlist wishlist)
        {
            var responce = _wishlistRepository.Create(wishlist);
            return responce;
        }

        public Task<BaseResponse<long>> Delete(Wishlist wishlist)
        {
            var responce = _wishlistRepository.Delete(wishlist);
            return responce;
        }

        public Task<BaseResponse<List<Wishlist>>> Get(Wishlist wishlist, int PageIndex, int PageSize, string Mode)
        {
            var responce = _wishlistRepository.Get(wishlist, PageIndex, PageSize, Mode);
            return responce;
        }

        public Task<BaseResponse<long>> Update(Wishlist wishlist)
        {
            var responce = _wishlistRepository.Update(wishlist);
            return responce;
        }
    }
}

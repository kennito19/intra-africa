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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Task<BaseResponse<long>> Create(Cart cart)
        {
            var responce = _cartRepository.Create(cart);
            return responce;
        }

        public Task<BaseResponse<long>> Delete(Cart cart, string? sellerProductIds = null)
        {
            var responce = _cartRepository.Delete(cart, sellerProductIds);
            return responce;
        }

        public Task<BaseResponse<List<Cart>>> Get(Cart cart, int PageIndex, int PageSize, string Mode, string? sellerProductIds = null)
        {
            var responce = _cartRepository.Get(cart, PageIndex, PageSize, Mode, sellerProductIds);
            return responce;
        }

        public Task<BaseResponse<long>> Update(Cart cart)
        {
            var responce = _cartRepository.Update(cart);
            return responce;
        }

        public Task<BaseResponse<long>> AddUserIdinCart(AddCartUserId cart)
        {
            var responce = _cartRepository.AddUserIdinCart(cart);
            return responce;
        }

        public Task<BaseResponse<List<Cart>>> GetAbandonedCart(Cart cart, int PageIndex, int PageSize, string Mode, string? sellerProductIds = null)
        {
            var response = _cartRepository.GetAbandonedCart(cart, PageIndex, PageSize, Mode, sellerProductIds);
            return response;
        }
    }
}

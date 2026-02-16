using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IRepositories
{
    public interface ICartRepository
    {
        Task<BaseResponse<long>> Create(Cart cart);
        Task<BaseResponse<long>> Update(Cart cart);
        Task<BaseResponse<long>> AddUserIdinCart(AddCartUserId cart);
        Task<BaseResponse<long>> Delete(Cart cart, string? sellerProductIds = null);
        Task<BaseResponse<List<Cart>>> Get(Cart cart, int PageIndex, int PageSize, string Mode, string? sellerProductIds = null);
        Task<BaseResponse<List<Cart>>> GetAbandonedCart(Cart cart, int PageIndex, int PageSize, string Mode, string? sellerProductIds = null);
    }
}

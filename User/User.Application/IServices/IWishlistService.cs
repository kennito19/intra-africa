using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IServices
{
    public interface IWishlistService
    {
        Task<BaseResponse<long>> Create(Wishlist wishlist);
        Task<BaseResponse<long>> Update(Wishlist wishlist);
        Task<BaseResponse<long>> Delete(Wishlist wishlist);
        Task<BaseResponse<List<Wishlist>>> Get(Wishlist wishlist, int PageIndex, int PageSize, string Mode);
    }
}

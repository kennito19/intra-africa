using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IProductsImagesRepository
    {
        Task<BaseResponse<long>> Create(ProductImages productImages);

        Task<BaseResponse<long>> Update(ProductImages productImages);

        Task<BaseResponse<long>> Delete(ProductImages productImages);

        Task<BaseResponse<List<ProductImages>>> get(ProductImages productImages, int PageIndex, int PageSize, string Mode);
    }
}

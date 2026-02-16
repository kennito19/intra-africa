using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IManageFlashSalePriceMasterRepository
    {
        Task<BaseResponse<long>> Create(FlashSalePriceMasterLibrary flashSalePrice);

        Task<BaseResponse<long>> Update(FlashSalePriceMasterLibrary flashSalePrice);

        Task<BaseResponse<long>> Delete(FlashSalePriceMasterLibrary flashSalePrice);

        Task<BaseResponse<List<FlashSalePriceMasterLibrary>>> get(FlashSalePriceMasterLibrary flashSalePrice, int PageIndex, int PageSize, string Mode);

    }
}

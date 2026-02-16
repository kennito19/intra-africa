using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ManageFlashSalePriceMasterService : IManageFlashSalePriceMasterService
    {
        private readonly IManageFlashSalePriceMasterRepository _flashSalePriceRepository;

        public ManageFlashSalePriceMasterService(IManageFlashSalePriceMasterRepository flashSalePrice)
        {
            _flashSalePriceRepository = flashSalePrice;
        }

        public Task<BaseResponse<long>> Create(FlashSalePriceMasterLibrary flashSalePrice)
        {
            var response = _flashSalePriceRepository.Create(flashSalePrice);
            return response;
        }

        public Task<BaseResponse<long>> Update(FlashSalePriceMasterLibrary flashSalePrice)
        {
            var response = _flashSalePriceRepository.Update(flashSalePrice);
            return response;
        }

        public Task<BaseResponse<long>> Delete(FlashSalePriceMasterLibrary flashSalePrice)
        {
            var reponse = _flashSalePriceRepository.Delete(flashSalePrice);
            return reponse;
        }

        public Task<BaseResponse<List<FlashSalePriceMasterLibrary>>> get(FlashSalePriceMasterLibrary flashSalePrice, int PageIndex, int PageSize, string Mode)
        {
            var response = _flashSalePriceRepository.get(flashSalePrice, PageIndex, PageSize, Mode);
            return response;
        }
    }
}

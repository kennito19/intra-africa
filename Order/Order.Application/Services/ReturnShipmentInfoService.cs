using Order.Application.IRepositories;
using Order.Application.IServices;
using Order.Domain;
using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public class ReturnShipmentInfoService: IReturnShipmentInfoService
    {
        private readonly IReturnShipmentInfoRepository _returnShipmentInfoRepository;

        public ReturnShipmentInfoService(IReturnShipmentInfoRepository returnShipmentInfoRepository)
        {
            _returnShipmentInfoRepository = returnShipmentInfoRepository;
        }

        Task<BaseResponse<long>> IReturnShipmentInfoService.Create(ReturnShipmentInfo returnShipmentInfo)
        {
            var response = _returnShipmentInfoRepository.Create(returnShipmentInfo);
            return response;
        }
        Task<BaseResponse<long>> IReturnShipmentInfoService.Update(ReturnShipmentInfo returnShipmentInfo)
        {
            var response = _returnShipmentInfoRepository.Update(returnShipmentInfo);
            return response;
        }

        Task<BaseResponse<long>> IReturnShipmentInfoService.Delete(ReturnShipmentInfo returnShipmentInfo)
        {
            var response = _returnShipmentInfoRepository.Delete(returnShipmentInfo);
            return response;
        }

        Task<BaseResponse<List<ReturnShipmentInfo>>> IReturnShipmentInfoService.Get(ReturnShipmentInfo returnShipmentInfo, int PageIndex, int PageSize, string Mode)
        {
            var response = _returnShipmentInfoRepository.Get(returnShipmentInfo, PageIndex, PageSize, Mode);
            return response;
        }

    }
}

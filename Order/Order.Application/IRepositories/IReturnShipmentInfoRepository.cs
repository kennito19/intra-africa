using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IReturnShipmentInfoRepository
    {
        Task<BaseResponse<long>> Create(ReturnShipmentInfo returnShipmentInfo);

        Task<BaseResponse<long>> Update(ReturnShipmentInfo returnShipmentInfo);

        Task<BaseResponse<long>> Delete(ReturnShipmentInfo returnShipmentInfo);

        Task<BaseResponse<List<ReturnShipmentInfo>>> Get(ReturnShipmentInfo returnShipmentInfo, int PageIndex, int PageSize, string Mode);

    }
}

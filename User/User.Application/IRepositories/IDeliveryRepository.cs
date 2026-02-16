using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IRepositories
{
    public interface IDeliveryRepository
    {
        Task<BaseResponse<long>> Create(DeliveryLibrary deliveryLibrary);
        Task<BaseResponse<long>> Update(DeliveryLibrary deliveryLibrary);
        Task<BaseResponse<long>> Delete(DeliveryLibrary deliveryLibrary);
        Task<BaseResponse<List<DeliveryLibrary>>> Get(DeliveryLibrary deliveryLibrary, int PageIndex, int PageSize, string Mode);
    }
}

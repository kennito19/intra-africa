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
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }
        public Task<BaseResponse<long>> Create(DeliveryLibrary deliveryLibrary)
        {
            var responce = _deliveryRepository.Create(deliveryLibrary);
            return responce;
        }
        public Task<BaseResponse<long>> Update(DeliveryLibrary deliveryLibrary)
        {
            var responce = _deliveryRepository.Update(deliveryLibrary);
            return responce;
        }
        public Task<BaseResponse<long>> Delete(DeliveryLibrary deliveryLibrary)
        {
            var responce = _deliveryRepository.Delete(deliveryLibrary);
            return responce;
        }

        public Task<BaseResponse<List<DeliveryLibrary>>> Get(DeliveryLibrary deliveryLibrary, int PageIndex, int PageSize, string Mode)
        {
            var responce = _deliveryRepository.Get(deliveryLibrary, PageIndex, PageSize, Mode);
            return responce;
        }

        
    }
}

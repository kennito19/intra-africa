using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Application.IServices;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public Task<BaseResponse<long>> Create(Warehouse warehouse)
        {
            var data = _warehouseRepository.Create(warehouse);
            return data;
        }
        public Task<BaseResponse<long>> Update(Warehouse warehouse)
        {
            var data = _warehouseRepository.Update(warehouse);
            return data;
        }
        public Task<BaseResponse<long>> Delete(Warehouse warehouse)
        {
            var data = _warehouseRepository.Delete(warehouse);
            return data;
        }

        public Task<BaseResponse<List<Warehouse>>> Get(Warehouse warehouse, int PageIndex, int PageSize, string Mode)
        {
            var data = _warehouseRepository.Get(warehouse, PageIndex, PageSize, Mode);
            return data;
        }

        
    }
}

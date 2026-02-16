using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.DTO;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ProductWarehouseService : IProductWarehouseService
    {
        private readonly IProductWarehouseRepository _productWarehouseRepository;

        public ProductWarehouseService(IProductWarehouseRepository productWarehouseRepository)
        {
            _productWarehouseRepository = productWarehouseRepository;
        }

        public Task<BaseResponse<long>> AddProductWarehouse(ProductWarehouse productWarehouse)
        {
            var responce = _productWarehouseRepository.AddProductWarehouse(productWarehouse);
            return responce;
        }

        public Task<BaseResponse<long>> DeleteProductWarehouse(ProductWarehouse productWarehouse)
        {
            var responce = _productWarehouseRepository.DeleteProductWarehouse(productWarehouse);
            return responce;
        }

        public Task<BaseResponse<List<ProductWarehouse>>> GetProductWarehouse(ProductWarehouse productWarehouse, int PageIndex, int PageSize, string Mode)
        {
            var responce = _productWarehouseRepository.GetProductWarehouse(productWarehouse, PageIndex, PageSize, Mode);
            return responce;
        }

        public Task<BaseResponse<long>> UpdateProductWarehouse(ProductWarehouse productWarehouse)
        {
            var responce = _productWarehouseRepository.UpdateProductWarehouse(productWarehouse);
            return responce;
        }

        public Task<BaseResponse<List<SizeWiseWarehouse>>> GetSizeWiseWarehouse(SizeWiseWarehouse productWarehouse)
        {
            var responce = _productWarehouseRepository.GetSizeWiseWarehouse(productWarehouse);
            return responce;
        }

        public Task<BaseResponse<long>> UpdateWarehouseStock(int warehouseid)
        {
            var responce = _productWarehouseRepository.UpdateWarehouseStock(warehouseid);
            return responce;
        }
    }
}

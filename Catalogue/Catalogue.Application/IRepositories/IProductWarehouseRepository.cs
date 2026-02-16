using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Domain.DTO;

namespace Catalogue.Application.IRepositories
{
    public interface IProductWarehouseRepository
    {
        Task<BaseResponse<long>> AddProductWarehouse(ProductWarehouse productWarehouse);

        Task<BaseResponse<long>> UpdateProductWarehouse(ProductWarehouse productWarehouse);

        Task<BaseResponse<long>> DeleteProductWarehouse(ProductWarehouse productWarehouse);

        Task<BaseResponse<List<ProductWarehouse>>> GetProductWarehouse(ProductWarehouse productWarehouse, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<List<SizeWiseWarehouse>>> GetSizeWiseWarehouse(SizeWiseWarehouse productWarehouse);
        Task<BaseResponse<long>> UpdateWarehouseStock(int warehouseid);
    }
}

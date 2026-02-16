using Catalogue.Domain.DTO;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IReportsRepository
    {
        Task<BaseResponse<List<ProductReport>>> GetProductReport(string? SellerId = null, string? fromDate = null, string? toDate = null);
        Task<BaseResponse<List<ProductDetailsReport>>> GetProductDetailsReport(string? productIds = null);

    }
}

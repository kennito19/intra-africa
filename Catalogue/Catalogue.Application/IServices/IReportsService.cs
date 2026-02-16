using Catalogue.Domain;
using Catalogue.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IReportsService
    {
        Task<BaseResponse<List<ProductReport>>> GetProductReport(string? SellerId = null, string? fromDate = null, string? toDate = null);
        Task<BaseResponse<List<ProductDetailsReport>>> GetProductDetailsReport(string? productIds = null);

    }
}

using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface ITaxMappingService
    {
        Task<BaseResponse<long>> AddTaxMapping(TaxMapping taxmap);

        Task<BaseResponse<long>> UpdateTaxMapping(TaxMapping taxmap);

        Task<BaseResponse<long>> DeleteTaxMapping(TaxMapping taxmap);

        Task<BaseResponse<List<TaxMapping>>> GetTaxMapping(TaxMapping taxmap, int PageIndex, int PageSize, string Mode);
    }
}

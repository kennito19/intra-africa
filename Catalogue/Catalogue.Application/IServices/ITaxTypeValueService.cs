using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface ITaxTypeValueService
    {
        Task<BaseResponse<long>> AddTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary);

        Task<BaseResponse<long>> UpdateTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary);

        Task<BaseResponse<long>> DeleteTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary);

        Task<BaseResponse<List<TaxTypeValueLibrary>>> GetTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary, int PageIndex, int PageSize, string Mode);
    }
}

using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface ITaxRateToHSNCodeService
    {
        Task<BaseResponse<long>> Create(AssignTaxRateToHSNCodeLibrary TaxRateToHsnCode);

        Task<BaseResponse<long>> Update(AssignTaxRateToHSNCodeLibrary TaxRateToHsnCode);

        Task<BaseResponse<long>> Delete(AssignTaxRateToHSNCodeLibrary TaxRateToHsnCode);

        Task<BaseResponse<List<AssignTaxRateToHSNCodeLibrary>>> get(AssignTaxRateToHSNCodeLibrary TaxRateToHsnCode, int PageIndex, int PageSize, string Mode);

    }
}

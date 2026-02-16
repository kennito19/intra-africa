using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface ITaxTypeRespository
    {
        Task<BaseResponse<long>> AddTaxType(TaxTypeLibrary taxTypeLibrary);

        Task<BaseResponse<long>> UpdateTaxType(TaxTypeLibrary taxTypeLibrary);

        Task<BaseResponse<long>> DeleteTaxType(TaxTypeLibrary taxTypeLibrary);

        Task<BaseResponse<List<TaxTypeLibrary>>> GetTaxType(TaxTypeLibrary taxTypeLibrary, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode);
    }
}

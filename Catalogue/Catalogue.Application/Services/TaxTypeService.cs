using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class TaxTypeService : ITaxTypeService
    {
        private readonly ITaxTypeRespository _taxTypeRespository;

        public TaxTypeService(ITaxTypeRespository taxTypeRespository)
        {
            _taxTypeRespository = taxTypeRespository;
        }

        public Task<BaseResponse<long>> AddTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            var data = _taxTypeRespository.AddTaxType(taxTypeLibrary);
            return data;
        }

        public Task<BaseResponse<long>> DeleteTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            var data = _taxTypeRespository.DeleteTaxType(taxTypeLibrary);
            return data;
        }

        public Task<BaseResponse<List<TaxTypeLibrary>>> GetTaxType(TaxTypeLibrary taxTypeLibrary, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode)
        {
            var data = _taxTypeRespository.GetTaxType(taxTypeLibrary, Getparent, Getchild, PageIndex, PageSize, Mode);
            return data;
        }

        public Task<BaseResponse<long>> UpdateTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            var data = _taxTypeRespository.UpdateTaxType(taxTypeLibrary);
            return data;
        }
    }
}

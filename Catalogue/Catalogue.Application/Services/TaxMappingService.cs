using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class TaxMappingService : ITaxMappingService
    {
        private readonly ITaxMappingRespository _taxmapRespository;

        public TaxMappingService(ITaxMappingRespository taxmapRespository)
        {
            _taxmapRespository = taxmapRespository;
        }

        public Task<BaseResponse<long>> AddTaxMapping(TaxMapping taxmap)
        {
            var data = _taxmapRespository.AddTaxMapping(taxmap);
            return data;
        }

        public Task<BaseResponse<long>> UpdateTaxMapping(TaxMapping taxmap)
        {
            var data = _taxmapRespository.UpdateTaxMapping(taxmap);
            return data;
        }
        public Task<BaseResponse<long>> DeleteTaxMapping(TaxMapping taxmap)
        {
            var data = _taxmapRespository.DeleteTaxMapping(taxmap);
            return data;
        }

        public Task<BaseResponse<List<TaxMapping>>> GetTaxMapping(TaxMapping taxmap, int PageIndex, int PageSize, string Mode)
        {
            var data = _taxmapRespository.GetTaxMapping(taxmap, PageIndex, PageSize, Mode);
            return data;
        }

    }
}

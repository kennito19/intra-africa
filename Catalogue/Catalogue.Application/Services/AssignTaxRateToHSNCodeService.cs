using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class AssignTaxRateToHSNCodeService : ITaxRateToHSNCodeService
    {
        private readonly ITaxRateToHSNCodeRepository _taxRateToHSNCodeRepository;

        public AssignTaxRateToHSNCodeService(ITaxRateToHSNCodeRepository taxRateToHSN)
        {
            _taxRateToHSNCodeRepository = taxRateToHSN;
        }

        public Task<BaseResponse<long>> Create(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            var response = _taxRateToHSNCodeRepository.Create(rateToHSNCode);
            return response;
        }

        public Task<BaseResponse<long>> Update(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            var response = _taxRateToHSNCodeRepository.Update(rateToHSNCode);
            return response;
        }

        public Task<BaseResponse<long>> Delete(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            var reponse = _taxRateToHSNCodeRepository.Delete(rateToHSNCode);
            return reponse;
        }

        public Task<BaseResponse<List<AssignTaxRateToHSNCodeLibrary>>> get(AssignTaxRateToHSNCodeLibrary rateToHSNCode, int PageIndex, int PageSize, string Mode)
        {
            var response = _taxRateToHSNCodeRepository.get(rateToHSNCode, PageIndex, PageSize, Mode);
            return response;
        }

    }
}

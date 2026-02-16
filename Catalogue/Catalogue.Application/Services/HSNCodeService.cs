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
    public class HSNCodeService : IHSNCodeService
    {
        private readonly IHSNCodeRepository _hSNCodeRepository;

        public HSNCodeService(IHSNCodeRepository hSNCodeRepository)
        {
            _hSNCodeRepository = hSNCodeRepository;
        }

        public Task<BaseResponse<long>> addHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            var data = _hSNCodeRepository.addHSNCode(hSNCodeLibrary);
            return data;
        }

        public Task<BaseResponse<long>> deleteHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            var data = _hSNCodeRepository.deleteHSNCode(hSNCodeLibrary);
            return data;
        }

        public Task<BaseResponse<List<HSNCodeLibrary>>> getHSNCode(HSNCodeLibrary hSNCodeLibrary, int PageIndex, int PageSize, string Mode)
        {
            var data = _hSNCodeRepository.getHSNCode(hSNCodeLibrary, PageIndex, PageSize, Mode);
            return data;
        }

        public Task<BaseResponse<long>> updateHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            var data = _hSNCodeRepository.updateHSNCode(hSNCodeLibrary);
            return data;
        }

    }
}

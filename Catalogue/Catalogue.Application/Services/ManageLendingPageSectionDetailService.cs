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
    public class ManageLendingPageSectionDetailService : IManageLendingPageSectionDetailsService
    {
        private readonly IManageLendingPageSectionDetailsRepository _lendingPageSectionDetailRepository;

        public ManageLendingPageSectionDetailService(IManageLendingPageSectionDetailsRepository lendingPageSectionDetailRepository)
        {
            _lendingPageSectionDetailRepository = lendingPageSectionDetailRepository;
        }

        public Task<BaseResponse<long>> Create(LendingPageSectionDetailsLibrary lendingPageSectionDetail)
        {
            var response = _lendingPageSectionDetailRepository.Create(lendingPageSectionDetail);
            return response;
        }

        public Task<BaseResponse<long>> Update(LendingPageSectionDetailsLibrary lendingPageSectionDetail)
        {
            var response = _lendingPageSectionDetailRepository.Update(lendingPageSectionDetail);
            return response;
        }

        public Task<BaseResponse<long>> Delete(LendingPageSectionDetailsLibrary lendingPageSectionDetail)
        {
            var reponse = _lendingPageSectionDetailRepository.Delete(lendingPageSectionDetail);
            return reponse;
        }

        public Task<BaseResponse<List<LendingPageSectionDetailsLibrary>>> get(LendingPageSectionDetailsLibrary lendingPageSectionDetail, int PageIndex, int PageSize, string Mode)
        {
            var response = _lendingPageSectionDetailRepository.get(lendingPageSectionDetail, PageIndex, PageSize, Mode);
            return response;
        }

    }
}

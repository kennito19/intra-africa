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
    public class ManageLendingPageService : IManageLendingPageService
    {
        private readonly IManageLendingPageRepository _lendingPageRepository;

        public ManageLendingPageService(IManageLendingPageRepository lendingPageRepository)
        {
            _lendingPageRepository = lendingPageRepository;
        }

        public Task<BaseResponse<long>> Create(LendingPageLibrary lendingPage)
        {
            var response = _lendingPageRepository.Create(lendingPage);
            return response;
        }

        public Task<BaseResponse<long>> Update(LendingPageLibrary lendingPage)
        {
            var response = _lendingPageRepository.Update(lendingPage);
            return response;
        }

        public Task<BaseResponse<long>> Delete(LendingPageLibrary lendingPage)
        {
            var reponse = _lendingPageRepository.Delete(lendingPage);
            return reponse;
        }

        public Task<BaseResponse<List<LendingPageLibrary>>> get(LendingPageLibrary lendingPage, int PageIndex, int PageSize, string Mode)
        {
            var response = _lendingPageRepository.get(lendingPage, PageIndex, PageSize, Mode);
            return response;
        }

    }
}

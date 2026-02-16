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
    public class ManageHomePageService : IManageHomePageService
    {
        private readonly IManageHomePageRepository _manageHomePageRepository;

        public ManageHomePageService(IManageHomePageRepository manageHomePageRepository)
        {
            _manageHomePageRepository = manageHomePageRepository;
        }
        public Task<BaseResponse<long>> Create(ManageHomePage manageHomePage)
        {
            var data=_manageHomePageRepository.Create(manageHomePage);
            return data;
        }
        public Task<BaseResponse<long>> Update(ManageHomePage manageHomePage)
        {
            var data=_manageHomePageRepository.Update(manageHomePage);
            return data;
        }

        public Task<BaseResponse<long>> Delete(ManageHomePage manageHomePage)
        {
            var data=_manageHomePageRepository.Delete(manageHomePage);
            return data;
        }

        public Task<BaseResponse<List<ManageHomePage>>> get(ManageHomePage manageHomePage, int PageIndex, int PageSize, string Mode)
        {
            var data = _manageHomePageRepository.get(manageHomePage, PageIndex, PageSize, Mode);
            return data;
        }

    }
}

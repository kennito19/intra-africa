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
    public class ManageHomePageSectionsService : IManageHomePageSectionsService
    {
        private readonly IManageHomePageSectionsRepository _homePageSectionRepository;

        public ManageHomePageSectionsService(IManageHomePageSectionsRepository homepagesectionRepository)
        {
            _homePageSectionRepository = homepagesectionRepository;
        }

        public Task<BaseResponse<long>> Create(ManageHomePageSectionsLibrary homePageSection)
        {
            var response = _homePageSectionRepository.Create(homePageSection);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageHomePageSectionsLibrary homePageSection)
        {
            var response = _homePageSectionRepository.Update(homePageSection);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageHomePageSectionsLibrary homePageSection)
        {
            var reponse = _homePageSectionRepository.Delete(homePageSection);
            return reponse;
        }

        public Task<BaseResponse<List<ManageHomePageSectionsLibrary>>> get(ManageHomePageSectionsLibrary homePageSection, int PageIndex, int PageSize, string Mode)
        {
            var response = _homePageSectionRepository.get(homePageSection, PageIndex, PageSize, Mode);
            return response;
        }

        public Task<BaseResponse<List<ProductHomePageSectionLibrary>>> getProudctHomePageSection(ProductHomePageSectionLibrary productHomePageSection, string Mode)
        {
            var response = _homePageSectionRepository.getProudctHomePageSection(productHomePageSection, Mode);
            return response;
        }

    }
}

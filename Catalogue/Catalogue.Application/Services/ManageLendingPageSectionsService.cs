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
    public class ManageLendingPageSectionsService : IManageLendingPageSectionsService
    {
        private readonly IManageLendingPageSectionsRepository _lendingPageSectionRepository;

        public ManageLendingPageSectionsService(IManageLendingPageSectionsRepository lendingPageSectionRepository)
        {
            _lendingPageSectionRepository = lendingPageSectionRepository;
        }

        public Task<BaseResponse<long>> Create(LendingPageSectionsLibrary lendingPageSection)
        {
            var response = _lendingPageSectionRepository.Create(lendingPageSection);
            return response;
        }

        public Task<BaseResponse<long>> Update(LendingPageSectionsLibrary lendingPageSection)
        {
            var response = _lendingPageSectionRepository.Update(lendingPageSection);
            return response;
        }

        public Task<BaseResponse<long>> Delete(LendingPageSectionsLibrary lendingPageSection)
        {
            var reponse = _lendingPageSectionRepository.Delete(lendingPageSection);
            return reponse;
        }

        public Task<BaseResponse<List<LendingPageSectionsLibrary>>> get(LendingPageSectionsLibrary lendingPageSection, int PageIndex, int PageSize, string Mode)
        {
            var response = _lendingPageSectionRepository.get(lendingPageSection, PageIndex, PageSize, Mode);
            return response;
        }

    }
}

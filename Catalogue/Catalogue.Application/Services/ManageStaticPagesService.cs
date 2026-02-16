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
    public class ManageStaticPagesService : IManageStaticPagesService
    {
        private readonly IManageStaticPagesRepository _staticPagesRepository;

        public ManageStaticPagesService(IManageStaticPagesRepository staticPagesRepository)
        {
            _staticPagesRepository = staticPagesRepository;
        }

        public Task<BaseResponse<long>> Create(ManageStaticPagesLibrary staticPages)
        {
            var response = _staticPagesRepository.Create(staticPages);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageStaticPagesLibrary staticPages)
        {
            var response = _staticPagesRepository.Update(staticPages);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageStaticPagesLibrary staticPages)
        {
            var reponse = _staticPagesRepository.Delete(staticPages);
            return reponse;
        }

        public Task<BaseResponse<List<ManageStaticPagesLibrary>>> get(ManageStaticPagesLibrary staticPages, int PageIndex, int PageSize, string Mode)
        {
            var response = _staticPagesRepository.get(staticPages, PageIndex, PageSize, Mode);
            return response;
        }

    }
}

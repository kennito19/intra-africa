using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Domain.DTO;

namespace Catalogue.Application.Services
{
    public class ManageHomePageDetailsService : IManageHomePageDetailsService
    {
        private readonly IManageHomePageDetailsRepository _homePageDedtailRepository;

        public ManageHomePageDetailsService(IManageHomePageDetailsRepository homepageDetailRepository)
        {
            _homePageDedtailRepository = homepageDetailRepository;
        }

        public Task<BaseResponse<long>> Create(ManageHomePageDetailsLibrary homePageDetail)
        {
            var response = _homePageDedtailRepository.Create(homePageDetail);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageHomePageDetailsLibrary homePageDetail)
        {
            var response = _homePageDedtailRepository.Update(homePageDetail);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageHomePageDetailsLibrary homePageDetail)
        {
            var reponse = _homePageDedtailRepository.Delete(homePageDetail);
            return reponse;
        }

        public Task<BaseResponse<List<ManageHomePageDetailsLibrary>>> get(ManageHomePageDetailsLibrary homePageDetail, int PageIndex, int PageSize, string Mode)
        {
            var response = _homePageDedtailRepository.get(homePageDetail, PageIndex, PageSize, Mode);
            return response;
        }
        public Task<BaseResponse<List<FrontHomepageDetailsDto>>> GetFrontHomepageDetails(string Mode, string? Status = null)
        {
            var response = _homePageDedtailRepository.GetFrontHomepageDetails(Mode, Status);
            return response;
        }
    }
}

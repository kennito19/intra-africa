using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Domain.DTO;

namespace Catalogue.Application.IRepositories
{
    public interface IManageHomePageDetailsRepository
    {
        Task<BaseResponse<long>> Create(ManageHomePageDetailsLibrary pageDetails);

        Task<BaseResponse<long>> Update(ManageHomePageDetailsLibrary pageDetails);

        Task<BaseResponse<long>> Delete(ManageHomePageDetailsLibrary pageDetails);

        Task<BaseResponse<List<ManageHomePageDetailsLibrary>>> get(ManageHomePageDetailsLibrary pageDetails, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<List<FrontHomepageDetailsDto>>> GetFrontHomepageDetails(string Mode, string? Status = null);
    }
}

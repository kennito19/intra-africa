using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IManageStaticPagesService
    {
        Task<BaseResponse<long>> Create(ManageStaticPagesLibrary staticPages);

        Task<BaseResponse<long>> Update(ManageStaticPagesLibrary staticPages);

        Task<BaseResponse<long>> Delete(ManageStaticPagesLibrary staticPages);

        Task<BaseResponse<List<ManageStaticPagesLibrary>>> get(ManageStaticPagesLibrary staticPages, int PageIndex, int PageSize, string Mode);
    }
}

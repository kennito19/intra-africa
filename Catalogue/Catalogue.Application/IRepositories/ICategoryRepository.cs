using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface ICategoryRepository
    {
        Task<BaseResponse<long>> Create(CategoryLibrary category);

        Task<BaseResponse<long>> Update(CategoryLibrary category);

        Task<BaseResponse<long>> Delete(CategoryLibrary category);

        Task<BaseResponse<List<CategoryLibrary>>> get(CategoryLibrary category, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<List<CategoryLibrary>>> GetCategoryWithParent(int Categoryid);

    }
}

using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface ISpecificationRepository
    {
        Task<BaseResponse<long>> Create(SpecificationLibrary specification);

        Task<BaseResponse<long>> Update(SpecificationLibrary specification);

        Task<BaseResponse<long>> Delete(SpecificationLibrary specification);

        Task<BaseResponse<List<SpecificationLibrary>>> get(SpecificationLibrary specification, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode);
    }
}

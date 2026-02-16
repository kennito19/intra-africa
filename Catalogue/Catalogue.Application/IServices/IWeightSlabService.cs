using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IWeightSlabService
    {
        Task<BaseResponse<long>> Create(WeightSlabLibrary weightSlab);

        Task<BaseResponse<long>> Update(WeightSlabLibrary weightSlab);

        Task<BaseResponse<long>> Delete(WeightSlabLibrary weightSlab);

        Task<BaseResponse<List<WeightSlabLibrary>>> get(WeightSlabLibrary weightSlab, int PageIndex, int PageSize, string Mode);
    }
}

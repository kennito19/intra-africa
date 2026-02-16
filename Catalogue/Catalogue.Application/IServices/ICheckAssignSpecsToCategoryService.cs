using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface ICheckAssignSpecsToCategoryService
    {
        Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSpecToCat(int assignSpecId, bool? multiSeller = false);
        Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSizeValuesToCat(int assignSpecId, int sizeTypeId, bool? multiSeller = false);
        Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkSizeType(int assignSpecId, int sizeTypeId);

        Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSpecvaluesToCat(int assignSpecId, int specTypeId, bool? multiSeller = false);
        Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkSpecType(int assignSpecId, int specTypeId);
    }
}

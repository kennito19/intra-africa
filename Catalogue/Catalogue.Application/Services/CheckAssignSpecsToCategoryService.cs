using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class CheckAssignSpecsToCategoryService : ICheckAssignSpecsToCategoryService
    {
        private readonly ICheckAssignSpecsToCategoryRepository _checkAssignSpecsToCategoryRepository;

        public CheckAssignSpecsToCategoryService(ICheckAssignSpecsToCategoryRepository checkAssignSpecsToCategoryRepository)
        {
            _checkAssignSpecsToCategoryRepository = checkAssignSpecsToCategoryRepository;
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSpecToCat(int assignSpecId, bool? multiSeller = false)
        {
            var data = await _checkAssignSpecsToCategoryRepository.checkAssignSpecToCat(assignSpecId, multiSeller);
            return data;
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSizeValuesToCat(int assignSpecId, int sizeTypeId, bool? multiSeller = false)
        {
            var data = await _checkAssignSpecsToCategoryRepository.checkAssignSizeValuesToCat(assignSpecId, sizeTypeId, multiSeller);
            return data;
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkSizeType(int assignSpecId, int sizeTypeId)
        {
            var data = await _checkAssignSpecsToCategoryRepository.checkSizeType(assignSpecId, sizeTypeId);
            return data;
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSpecvaluesToCat(int assignSpecId, int specTypeId, bool? multiSeller = false)
        {
            var data = await _checkAssignSpecsToCategoryRepository.checkAssignSpecvaluesToCat(assignSpecId, specTypeId, multiSeller);
            return data;
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkSpecType(int assignSpecId, int specTypeId)
        {
            var data = await _checkAssignSpecsToCategoryRepository.checkSpecType(assignSpecId, specTypeId);
            return data;
        }

       
    }
}

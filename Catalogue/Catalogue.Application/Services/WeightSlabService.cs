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
    public class WeightSlabService : IWeightSlabService
    {
        private readonly IWeightSlabRepository _weightSlabRepository;
        public WeightSlabService(IWeightSlabRepository weightSlabRepository)
        {
            _weightSlabRepository = weightSlabRepository;
        }
        public Task<BaseResponse<long>> Create(WeightSlabLibrary weightSlab)
        {
            var response = _weightSlabRepository.Create(weightSlab);
            return response;
        }
        public Task<BaseResponse<long>> Update(WeightSlabLibrary weightSlab)
        {
            var response = _weightSlabRepository.Update(weightSlab);
            return response;
        }
        public Task<BaseResponse<long>> Delete(WeightSlabLibrary weightSlab)
        {
            var response = _weightSlabRepository.Delete(weightSlab);
            return response;
        }

        public Task<BaseResponse<List<WeightSlabLibrary>>> get(WeightSlabLibrary weightSlab, int PageIndex, int PageSize, string Mode)
        {
            var response = _weightSlabRepository.get(weightSlab, PageIndex, PageSize, Mode);
            return response;
        }

        
    }
}

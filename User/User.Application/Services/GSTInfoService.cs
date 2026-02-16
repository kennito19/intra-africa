using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Application.IServices;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.Services
{
    public class GSTInfoService : IGSTInfoService
    {
        private readonly IGSTInfoRepository _gSTInfoRepository;

        public GSTInfoService(IGSTInfoRepository gSTInfoRepository)
        {
            _gSTInfoRepository = gSTInfoRepository;
        }

        public Task<BaseResponse<long>> Create(GSTInfo gSTInfo)
        {
            var data = _gSTInfoRepository.Create(gSTInfo);
            return data;
        }
        public Task<BaseResponse<long>> Update(GSTInfo gSTInfo)
        {
            var data = _gSTInfoRepository.Update(gSTInfo);
            return data;
        }

        public Task<BaseResponse<long>> Delete(GSTInfo gSTInfo)
        {
            var data = _gSTInfoRepository.Delete(gSTInfo);
            return data;
        }

        public Task<BaseResponse<List<GSTInfo>>> Get(GSTInfo gSTInfo, int PageIndex, int PageSize, string Mode)
        {
            var data = _gSTInfoRepository.Get(gSTInfo, PageIndex, PageSize, Mode);
            return data;
        }

        
    }
}

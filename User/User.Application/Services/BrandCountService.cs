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
    public class BrandCountService : IBrandCountService
    {
        private readonly IBrandCountRepository _brandCountRepository;

        public BrandCountService(IBrandCountRepository brandCountRepository)
        {
            _brandCountRepository = brandCountRepository;
        }

        public async Task<BaseResponse<List<BrandCounts>>> get(string? sellerId, string? days)
        {
            var data = await _brandCountRepository.get(sellerId, days);
            return data;
        }
    }
}

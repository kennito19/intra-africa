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
    public class KycCountService : IKycCountService
    {
        private readonly IKycCountRepository _kycCountRepository;

        public KycCountService(IKycCountRepository kycCountRepository)
        {
            _kycCountRepository = kycCountRepository;
        }
        public async Task<BaseResponse<List<KycCounts>>> get(string? days)
        {
            var data = await _kycCountRepository.get(days);
            return data;
        }
    }
}

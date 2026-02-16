using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IServices
{
    public interface IKycCountService
    {
        Task<BaseResponse<List<KycCounts>>> get(string? days);
    }
}

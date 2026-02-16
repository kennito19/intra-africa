using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.IRepositories
{
    public interface IKycCountRepository
    {
        Task<BaseResponse<List<KycCounts>>> get(string? days);
    }
}

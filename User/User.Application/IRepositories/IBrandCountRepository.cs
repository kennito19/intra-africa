using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;

namespace User.Application.IRepositories
{
    public interface IBrandCountRepository
    {
        Task<BaseResponse<List<BrandCounts>>> get(string? sellerId ,string? days);
    }
}

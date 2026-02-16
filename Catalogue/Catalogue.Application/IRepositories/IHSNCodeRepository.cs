using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IHSNCodeRepository
    {
        Task<BaseResponse<long>> addHSNCode(HSNCodeLibrary hSNCodeLibrary);
        Task<BaseResponse<long>> updateHSNCode(HSNCodeLibrary hSNCodeLibrary);
        Task<BaseResponse<List<HSNCodeLibrary>>> getHSNCode(HSNCodeLibrary hSNCodeLibrary, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<long>> deleteHSNCode(HSNCodeLibrary hSNCodeLibrary);
    }
}

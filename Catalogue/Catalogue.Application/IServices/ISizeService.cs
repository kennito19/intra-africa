using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface ISizeService
    {
        Task<BaseResponse<long>> Create(SizeLibrary size);
        Task<BaseResponse<long>> Update(SizeLibrary size);
        Task<BaseResponse<long>> Delete(SizeLibrary size);
        Task<BaseResponse<List<SizeLibrary>>> Get(SizeLibrary size, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode);
     }
}

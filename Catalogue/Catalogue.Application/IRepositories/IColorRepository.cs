using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IColorRepository
    {
        Task<BaseResponse<long>> Create(ColorLibrary color);

        Task<BaseResponse<long>> Update(ColorLibrary color);

        Task<BaseResponse<long>> Delete(ColorLibrary color);

        Task<BaseResponse<List<ColorLibrary>>> get(ColorLibrary color, int PageIndex, int PageSize, string Mode);
    }
}

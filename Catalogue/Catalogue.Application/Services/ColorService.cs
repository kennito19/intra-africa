using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Application.IServices;

namespace Catalogue.Application.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public Task<BaseResponse<long>> Create(ColorLibrary color)
        {
            var response = _colorRepository.Create(color);
            return response;
        }

        public Task<BaseResponse<long>> Update(ColorLibrary color)
        {
            var response = _colorRepository.Update(color);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ColorLibrary color)
        {
            var reponse = _colorRepository.Delete(color);
            return reponse;
        }

        public Task<BaseResponse<List<ColorLibrary>>> get(ColorLibrary color, int PageIndex, int PageSize, string Mode)
        {
            var response = _colorRepository.get(color, PageIndex, PageSize, Mode);
            return response;
        }
    }
}

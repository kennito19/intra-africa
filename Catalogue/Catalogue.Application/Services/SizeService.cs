using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;

        public SizeService(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        public Task<BaseResponse<long>> Create(SizeLibrary size)
        {
            var response = _sizeRepository.Create(size);
            return response;
        }
        public Task<BaseResponse<long>> Update(SizeLibrary size)
        {
            var response = _sizeRepository.Update(size);
            return response;
        }

        public Task<BaseResponse<long>> Delete(SizeLibrary size)
        {
            var response = _sizeRepository.Delete(size);
            return response;
        }

        public Task<BaseResponse<List<SizeLibrary>>> Get(SizeLibrary size, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode)
        {
            var response = _sizeRepository.Get(size, Getparent, Getchild, PageIndex, PageSize, Mode);
            return response;
        }

    }
}

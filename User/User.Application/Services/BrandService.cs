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
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;

        public BrandService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public Task<BaseResponse<long>> Create(Brand brand)
        {
            var data = _brandRepository.Create(brand);
            return data;
        }
        public Task<BaseResponse<long>> Update(Brand brand)
        {
            var data = _brandRepository.Update(brand);
            return data;
        }

        public Task<BaseResponse<long>> Delete(Brand brand)
        {
            var data = _brandRepository.Delete(brand);
            return data;
        }

        public Task<BaseResponse<List<Brand>>> Get(Brand brand, int PageIndex, int PageSize, string Mode)
        {
            var data = _brandRepository.Get(brand, PageIndex, PageSize, Mode);
            return data;
        }

    }
}

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
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public Task<BaseResponse<long>> Create(Address address)
        {
            var data = _addressRepository.Create(address);
            return data;
        }

        public Task<BaseResponse<long>> Update(Address address)
        {
            var data = _addressRepository.Update(address);
            return data;
        }
        
        public Task<BaseResponse<long>> Delete(Address address)
        {
            var data = _addressRepository.Delete(address);
            return data;
        }

        public Task<BaseResponse<List<Address>>> Get(Address address, int PageIndex, int PageSize, string Mode)
        {
            var data = _addressRepository.Get(address, PageIndex, PageSize, Mode);
            return data;
        }

    }
}

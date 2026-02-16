using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class GetCartListService : IGetCartListService
    {
        private readonly IGetCartListRepository _getCartListRepository;
        public GetCartListService(IGetCartListRepository getCartListRepository)
        {
            _getCartListRepository = getCartListRepository;
        }
        public Task<BaseResponse<List<GetCheckOutDetailsList>>> GetCartList(getChekoutCalculation cart)
        {
            var response = _getCartListRepository.GetCartList(cart);
            return response;
        }
    }
}

using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Domain.DTO;

namespace Catalogue.Application.IRepositories
{
    public interface IGetCartListRepository
    {
        Task<BaseResponse<List<GetCheckOutDetailsList>>> GetCartList(getChekoutCalculation cart);
    }
}

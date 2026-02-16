using Catalogue.Domain.DTO;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IGetCartListService
    {
        Task<BaseResponse<List<GetCheckOutDetailsList>>> GetCartList(getChekoutCalculation cart);
    }
}

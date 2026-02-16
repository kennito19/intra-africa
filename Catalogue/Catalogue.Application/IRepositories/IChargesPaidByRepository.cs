using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IChargesPaidByRepository
    {
        Task<BaseResponse<long>> Create(ChargesPaidByLibrary chargesPaidBy);

        Task<BaseResponse<long>> Update(ChargesPaidByLibrary chargesPaidBy);

        Task<BaseResponse<long>> Delete(ChargesPaidByLibrary chargesPaidBy);

        Task<BaseResponse<List<ChargesPaidByLibrary>>> get(ChargesPaidByLibrary chargesPaidBy, int PageIndex, int PageSize, string Mode);
    }
}

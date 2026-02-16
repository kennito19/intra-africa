using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface ICommissionChargesRepository
    {
        Task<BaseResponse<long>> Create(CommissionChargesLibrary commissionCharges);

        Task<BaseResponse<long>> Update(CommissionChargesLibrary commissionCharges);

        Task<BaseResponse<long>> Delete(CommissionChargesLibrary commissionCharges);

        Task<BaseResponse<List<CommissionChargesLibrary>>> get(CommissionChargesLibrary commissionCharges, int PageIndex, int PageSize, string Mode);

        Task<BaseResponse<List<CommissionChargesLibrary>>> getCategoryWiseCommission(CommissionChargesLibrary commissionCharges);
    }
}

using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
	public interface IManageOffersMappingService
	{
		Task<BaseResponse<long>> Create(ManageOffersMappingLibrary model);

		Task<BaseResponse<long>> Update(ManageOffersMappingLibrary model);

		Task<BaseResponse<long>> Delete(ManageOffersMappingLibrary model);

		Task<BaseResponse<List<ManageOffersMappingLibrary>>> get(ManageOffersMappingLibrary model, int PageIndex, int PageSize, string Mode);
	}
}

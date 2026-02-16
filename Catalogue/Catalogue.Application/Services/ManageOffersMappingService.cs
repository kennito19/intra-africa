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
	public class ManageOffersMappingService : IManageOffersMappingService
	{
		private readonly IManageOffersMappingRepository _manageOffersMapping;

		public ManageOffersMappingService(IManageOffersMappingRepository manageOffersMapping)
        {
			_manageOffersMapping = manageOffersMapping;
		}
        public Task<BaseResponse<long>> Create(ManageOffersMappingLibrary model)
		{
			var res = _manageOffersMapping.Create(model);
			return res;
		}

		public Task<BaseResponse<long>> Update(ManageOffersMappingLibrary model)
		{
			var res = _manageOffersMapping.Update(model);
			return res;
		}

		public Task<BaseResponse<long>> Delete(ManageOffersMappingLibrary model)
		{
			var res = _manageOffersMapping.Delete(model);
			return res;
		}

		public Task<BaseResponse<List<ManageOffersMappingLibrary>>> get(ManageOffersMappingLibrary model, int PageIndex, int PageSize, string Mode)
		{
			var res = _manageOffersMapping.get(model, PageIndex, PageSize, Mode);
			return res;
		}
	}
}

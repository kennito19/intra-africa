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
	public class ManageOffersService : IManageOffersService
	{
		private readonly IManageOffersRepository _manageOffersRepository;

		public ManageOffersService(IManageOffersRepository manageOffersRepository)
        {
			_manageOffersRepository = manageOffersRepository;
		}
        public Task<BaseResponse<long>> Create(ManageOffersLibrary model)
		{
			var res=_manageOffersRepository.Create(model);
			return res;
		}

		public Task<BaseResponse<long>> Delete(ManageOffersLibrary model)
		{
			var res=_manageOffersRepository.Delete(model);
			return res;
		}

		public Task<BaseResponse<List<ManageOffersLibrary>>> get(ManageOffersLibrary model, int PageIndex, int PageSize, string Mode)
		{
			var res = _manageOffersRepository.get(model, PageIndex, PageSize, Mode);
			return res;
		}

		public Task<BaseResponse<long>> Update(ManageOffersLibrary model)
		{
			var res = _manageOffersRepository.Update(model);
			return res;
		}
	}
}

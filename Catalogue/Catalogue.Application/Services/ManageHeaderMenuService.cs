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
	public class ManageHeaderMenuService : IManageHeaderMenuService
	{
		private readonly IManageHeaderMenuRepository _manageHeaderMenuRepository;

		public ManageHeaderMenuService(IManageHeaderMenuRepository manageHeaderMenuRepository)
        {
			_manageHeaderMenuRepository = manageHeaderMenuRepository;
		}
        public Task<BaseResponse<long>> Create(ManageHeaderMenu model)
		{
			var data = _manageHeaderMenuRepository.Create(model);
			return data;
		}

		public Task<BaseResponse<long>> Delete(ManageHeaderMenu model)
		{
			var data = _manageHeaderMenuRepository.Delete(model);
			return data;
		}

		public Task<BaseResponse<List<ManageHeaderMenu>>> get(ManageHeaderMenu model, int PageIndex, int PageSize, string Mode)
		{
			var data = _manageHeaderMenuRepository.get(model, PageIndex, PageSize, Mode);
			return data;
		}

		public Task<BaseResponse<long>> Update(ManageHeaderMenu model)
		{
			var data = _manageHeaderMenuRepository.Update(model);
			return data;
		}
	}
}

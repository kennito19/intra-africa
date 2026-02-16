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
	public class ManageSubMenuService : IManageSubMenuService
	{
		private readonly IManageSubMenuRepository _manageSubMenuRepository;

		public ManageSubMenuService(IManageSubMenuRepository manageSubMenuRepository)
        {
			_manageSubMenuRepository = manageSubMenuRepository;
		}
        public Task<BaseResponse<long>> Create(ManageSubMenu model)
		{
			var data = _manageSubMenuRepository.Create(model);
			return data;
		}

		public Task<BaseResponse<long>> Delete(ManageSubMenu model)
		{
			var data = _manageSubMenuRepository.Delete(model);
			return data;
		}

		public Task<BaseResponse<List<ManageSubMenu>>> get(ManageSubMenu model, int PageIndex, int PageSize, string Mode, string HeaderName, string ParentName, bool getParent, bool getChild)
		{
			var data = _manageSubMenuRepository.get(model, PageIndex, PageSize, Mode, HeaderName, ParentName, getParent, getChild);
			return data;
		}

		public Task<BaseResponse<long>> Update(ManageSubMenu model)
		{
			var data = _manageSubMenuRepository.Update(model);
			return data;
		}
	}
}

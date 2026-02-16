using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
	public interface IManageSubMenuService
	{
		Task<BaseResponse<long>> Create(ManageSubMenu model);

		Task<BaseResponse<long>> Update(ManageSubMenu model);

		Task<BaseResponse<long>> Delete(ManageSubMenu model);

		Task<BaseResponse<List<ManageSubMenu>>> get(ManageSubMenu model, int PageIndex, int PageSize, string Mode, string HeaderName, string ParentName, bool getParent, bool getChild);
	}
}

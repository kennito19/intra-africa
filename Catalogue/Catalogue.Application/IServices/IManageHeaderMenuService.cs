using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Catalogue.Application.IServices
{
	public interface IManageHeaderMenuService
	{
		Task<BaseResponse<long>> Create(ManageHeaderMenu model);

		Task<BaseResponse<long>> Update(ManageHeaderMenu model);

		Task<BaseResponse<long>> Delete(ManageHeaderMenu model);

		Task<BaseResponse<List<ManageHeaderMenu>>> get(ManageHeaderMenu model, int PageIndex, int PageSize, string Mode);
	}
}

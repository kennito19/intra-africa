using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Catalogue.Application.IRepositories
{
	public interface IManageOffersRepository
	{
		Task<BaseResponse<long>> Create(ManageOffersLibrary model);

		Task<BaseResponse<long>> Update(ManageOffersLibrary model);

		Task<BaseResponse<long>> Delete(ManageOffersLibrary model);

		Task<BaseResponse<List<ManageOffersLibrary>>> get(ManageOffersLibrary model, int PageIndex, int PageSize, string Mode);
	}
}

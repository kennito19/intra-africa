using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
	public class ManageSubMenuRepository : IManageSubMenuRepository
	{
		private readonly SqlConnection con;
		private readonly IConfiguration _configuration;
		private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

		public ManageSubMenuRepository(IConfiguration configuration)
        {
			string connectionString = configuration.GetConnectionString("DBconnection");
			con = new SqlConnection(connectionString);

			_configuration = configuration;
		}
        public async Task<BaseResponse<long>> Create(ManageSubMenu model)
		{
			try
			{
				var sqlParams = new List<SqlParameter>()
				{
					new SqlParameter("@mode","add"),
					new SqlParameter("@menuType",model.MenuType),
					new SqlParameter("@headerId",model.HeaderId),
					new SqlParameter("@parentId",model.ParentId),
					new SqlParameter("@name",model.Name),
					new SqlParameter("@image",model.Image),
					new SqlParameter("@imageAlt", model.ImageAlt),
					new SqlParameter("@haslink", model.HasLink),
					new SqlParameter("@redirectTo", model.RedirectTo),
					new SqlParameter("@lendingPageId", model.LendingPageId),
					new SqlParameter("@categoryId", model.CategoryId),
					new SqlParameter("@staticPageId", model.StaticPageId),
					new SqlParameter("@collectionId", model.CollectionId),
					new SqlParameter("@customLink", model.CustomLink),
					new SqlParameter("@sizes", model.Sizes),
					new SqlParameter("@specifications", model.Specifications),
					new SqlParameter("@colors", model.Colors),
					new SqlParameter("@brands", model.Brands),
					new SqlParameter("@sequence", model.Sequence),
					new SqlParameter("@createdBy", model.CreatedBy),
					new SqlParameter("@createdAt", model.CreatedAt)
				};

				SqlParameter output = new SqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.SqlDbType = SqlDbType.Int;

				SqlParameter newid = new SqlParameter();
				newid.ParameterName = "@newid";
				newid.Direction = ParameterDirection.Output;
				newid.SqlDbType = SqlDbType.BigInt;

				SqlParameter message = new SqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.SqlDbType = SqlDbType.NVarChar;
				message.Size = 50;

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageSubmenu, output, newid, message, sqlParams.ToArray());
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<BaseResponse<long>> Delete(ManageSubMenu model)
		{
			try
			{
				var sqlParams = new List<SqlParameter>() {
					new SqlParameter("@mode", "delete"),
					new SqlParameter("@id", model.Id)
				};

				SqlParameter output = new SqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.SqlDbType = SqlDbType.Int;

				SqlParameter newid = new SqlParameter();
				newid.ParameterName = "@newid";
				newid.Direction = ParameterDirection.Output;
				newid.SqlDbType = SqlDbType.BigInt;

				SqlParameter message = new SqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.SqlDbType = SqlDbType.NVarChar;
				message.Size = 50;

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageSubmenu, output, newid, message, sqlParams.ToArray());

			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<BaseResponse<List<ManageSubMenu>>> get(ManageSubMenu model, int PageIndex, int PageSize, string Mode,string HeaderName=null,string ParentName=null, bool getParent=false, bool getChild=false)
		{
			try
			{
				var sqlParams = new List<SqlParameter>() {
					new SqlParameter("@mode", Mode),
					new SqlParameter("@id", model.Id),
					new SqlParameter("@parentId", model.ParentId),
					new SqlParameter("@headerId", model.HeaderId),
					new SqlParameter("@CategoryId", model.CategoryId),
					new SqlParameter("@menutype", model.MenuType),
					new SqlParameter("@name",model.Name),
					new SqlParameter("@headerName",HeaderName),
					new SqlParameter("@parentName",ParentName),
					new SqlParameter("@getParent",getParent),
					new SqlParameter("@getChild",getChild),
					new SqlParameter("@searchtext", model.Searchtext),
					new SqlParameter("@pageIndex", PageIndex),
					new SqlParameter("@PageSize", PageSize)

				};

				SqlParameter output = new SqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.SqlDbType = SqlDbType.Int;

				SqlParameter message = new SqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.SqlDbType = SqlDbType.NVarChar;
				message.Size = 50;

				return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageSubmenu, ManageSubMenuAsync, output, newid: null, message, sqlParams.ToArray());

			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<BaseResponse<long>> Update(ManageSubMenu model)
		{
			try
			{
				var sqlParams = new List<SqlParameter>() {
					new SqlParameter("@mode", "update"),
					new SqlParameter("@id",model.Id),
					new SqlParameter("@menuType",model.MenuType),
					new SqlParameter("@headerId",model.HeaderId),
					new SqlParameter("@parentId",model.ParentId),
					new SqlParameter("@name",model.Name),
					new SqlParameter("@image",model.Image),
					new SqlParameter("@imageAlt", model.ImageAlt),
					new SqlParameter("@haslink", model.HasLink),
					new SqlParameter("@redirectTo", model.RedirectTo),
					new SqlParameter("@lendingPageId", model.LendingPageId),
					new SqlParameter("@categoryId", model.CategoryId),
					new SqlParameter("@staticPageId", model.StaticPageId),
					new SqlParameter("@collectionId", model.CollectionId),
					new SqlParameter("@customLink", model.CustomLink),
					new SqlParameter("@sizes", model.Sizes),
					new SqlParameter("@specifications", model.Specifications),
					new SqlParameter("@colors", model.Colors),
					new SqlParameter("@brands", model.Brands),
					new SqlParameter("@sequence", model.Sequence),
					new SqlParameter("@modifiedBy", model.ModifiedBy),
					new SqlParameter("@modifiedAt", model.ModifiedAt)
				};

				SqlParameter output = new SqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.SqlDbType = SqlDbType.Int;

				SqlParameter newid = new SqlParameter();
				newid.ParameterName = "@newid";
				newid.Direction = ParameterDirection.Output;
				newid.SqlDbType = SqlDbType.BigInt;

				SqlParameter message = new SqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.SqlDbType = SqlDbType.NVarChar;
				message.Size = 50;

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageSubmenu, output, newid, message, sqlParams.ToArray());

			}
			catch (Exception)
			{
				throw;
			}
		}

		private async Task<List<ManageSubMenu>> ManageSubMenuAsync(DbDataReader reader)
		{
			List<ManageSubMenu> manageSubMenu = new List<ManageSubMenu>();
			while (await reader.ReadAsync())
			{
				manageSubMenu.Add(new ManageSubMenu()
				{
					RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
					PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
					RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
					Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
					MenuType = Convert.ToString(reader.GetValue(reader.GetOrdinal("menuType"))),
					HeaderId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("headerId"))),
					ParentId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("parentId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("parentId"))),
					Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
					Image = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Image")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Image"))),
					ImageAlt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ImageAlt")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageAlt"))),
					HasLink = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("HasLink"))),
					RedirectTo = Convert.ToString(reader.GetValue(reader.GetOrdinal("RedirectTo"))),
					LendingPageId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LendingPageId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LendingPageId"))),
					CategoryId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CategoryId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
					StaticPageId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("StaticPageId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("StaticPageId"))),
					CollectionId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CollectionId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CollectionId"))),
					CustomLink = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CustomLink")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomLink"))),
					Sizes = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("sizes")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("sizes"))),
					Colors = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("colors")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("colors"))),
					Specifications = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("specifications")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("specifications"))),
					Brands = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("brands")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("brands"))),
					Sequence = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Sequence"))),
                    HeaderColor = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("HeaderColor")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HeaderColor"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
					CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
					ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
					ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt")))
				});
			}
			return manageSubMenu;
		}
	}
}

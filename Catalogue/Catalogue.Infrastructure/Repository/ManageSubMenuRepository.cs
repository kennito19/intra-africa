using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
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
				var sqlParams = new List<MySqlParameter>()
				{
					new MySqlParameter("@mode","add"),
					new MySqlParameter("@menuType",model.MenuType),
					new MySqlParameter("@headerId",model.HeaderId),
					new MySqlParameter("@parentId",model.ParentId),
					new MySqlParameter("@name",model.Name),
					new MySqlParameter("@image",model.Image),
					new MySqlParameter("@imageAlt", model.ImageAlt),
					new MySqlParameter("@haslink", model.HasLink),
					new MySqlParameter("@redirectTo", model.RedirectTo),
					new MySqlParameter("@lendingPageId", model.LendingPageId),
					new MySqlParameter("@categoryId", model.CategoryId),
					new MySqlParameter("@staticPageId", model.StaticPageId),
					new MySqlParameter("@collectionId", model.CollectionId),
					new MySqlParameter("@customLink", model.CustomLink),
					new MySqlParameter("@sizes", model.Sizes),
					new MySqlParameter("@specifications", model.Specifications),
					new MySqlParameter("@colors", model.Colors),
					new MySqlParameter("@brands", model.Brands),
					new MySqlParameter("@sequence", model.Sequence),
					new MySqlParameter("@createdBy", model.CreatedBy),
					new MySqlParameter("@createdAt", model.CreatedAt)
				};

				MySqlParameter output = new MySqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.MySqlDbType = MySqlDbType.Int32;

				MySqlParameter newid = new MySqlParameter();
				newid.ParameterName = "@newid";
				newid.Direction = ParameterDirection.Output;
				newid.MySqlDbType = MySqlDbType.Int64;

				MySqlParameter message = new MySqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.MySqlDbType = MySqlDbType.VarChar;
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
				var sqlParams = new List<MySqlParameter>() {
					new MySqlParameter("@mode", "delete"),
					new MySqlParameter("@id", model.Id)
				};

				MySqlParameter output = new MySqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.MySqlDbType = MySqlDbType.Int32;

				MySqlParameter newid = new MySqlParameter();
				newid.ParameterName = "@newid";
				newid.Direction = ParameterDirection.Output;
				newid.MySqlDbType = MySqlDbType.Int64;

				MySqlParameter message = new MySqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.MySqlDbType = MySqlDbType.VarChar;
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
				var sqlParams = new List<MySqlParameter>() {
					new MySqlParameter("@mode", Mode),
					new MySqlParameter("@id", model.Id),
					new MySqlParameter("@parentId", model.ParentId),
					new MySqlParameter("@headerId", model.HeaderId),
					new MySqlParameter("@CategoryId", model.CategoryId),
					new MySqlParameter("@menutype", model.MenuType),
					new MySqlParameter("@name",model.Name),
					new MySqlParameter("@headerName",HeaderName),
					new MySqlParameter("@parentName",ParentName),
					new MySqlParameter("@getParent",getParent),
					new MySqlParameter("@getChild",getChild),
					new MySqlParameter("@searchtext", model.Searchtext),
					new MySqlParameter("@pageIndex", PageIndex),
					new MySqlParameter("@PageSize", PageSize)

				};

				MySqlParameter output = new MySqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.MySqlDbType = MySqlDbType.Int32;

				MySqlParameter message = new MySqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.MySqlDbType = MySqlDbType.VarChar;
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
				var sqlParams = new List<MySqlParameter>() {
					new MySqlParameter("@mode", "update"),
					new MySqlParameter("@id",model.Id),
					new MySqlParameter("@menuType",model.MenuType),
					new MySqlParameter("@headerId",model.HeaderId),
					new MySqlParameter("@parentId",model.ParentId),
					new MySqlParameter("@name",model.Name),
					new MySqlParameter("@image",model.Image),
					new MySqlParameter("@imageAlt", model.ImageAlt),
					new MySqlParameter("@haslink", model.HasLink),
					new MySqlParameter("@redirectTo", model.RedirectTo),
					new MySqlParameter("@lendingPageId", model.LendingPageId),
					new MySqlParameter("@categoryId", model.CategoryId),
					new MySqlParameter("@staticPageId", model.StaticPageId),
					new MySqlParameter("@collectionId", model.CollectionId),
					new MySqlParameter("@customLink", model.CustomLink),
					new MySqlParameter("@sizes", model.Sizes),
					new MySqlParameter("@specifications", model.Specifications),
					new MySqlParameter("@colors", model.Colors),
					new MySqlParameter("@brands", model.Brands),
					new MySqlParameter("@sequence", model.Sequence),
					new MySqlParameter("@modifiedBy", model.ModifiedBy),
					new MySqlParameter("@modifiedAt", model.ModifiedAt)
				};

				MySqlParameter output = new MySqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.MySqlDbType = MySqlDbType.Int32;

				MySqlParameter newid = new MySqlParameter();
				newid.ParameterName = "@newid";
				newid.Direction = ParameterDirection.Output;
				newid.MySqlDbType = MySqlDbType.Int64;

				MySqlParameter message = new MySqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.MySqlDbType = MySqlDbType.VarChar;
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

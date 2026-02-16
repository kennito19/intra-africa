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
	public class ManageHeaderMenuRepository : IManageHeaderMenuRepository
	{
		private readonly SqlConnection con;
		private readonly IConfiguration _configuration;
		private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

		public ManageHeaderMenuRepository(IConfiguration configuration)
        {
			string connectionString = configuration.GetConnectionString("DBconnection");
			con = new SqlConnection(connectionString);

			_configuration = configuration;
		}
        public async Task<BaseResponse<long>> Create(ManageHeaderMenu model)
		{
			try
			{
				var sqlParams = new List<SqlParameter>()
				{
					new SqlParameter("@mode","add"),
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
					new SqlParameter("@sequence", model.Sequence),
					new SqlParameter("@color", model.color),
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

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHeadermenu, output, newid, message, sqlParams.ToArray());
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<BaseResponse<long>> Delete(ManageHeaderMenu model)
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

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHeadermenu, output, newid, message, sqlParams.ToArray());

			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<BaseResponse<List<ManageHeaderMenu>>> get(ManageHeaderMenu model, int PageIndex, int PageSize, string Mode)
		{
			try
			{
				var sqlParams = new List<SqlParameter>() {
					new SqlParameter("@mode", Mode),
					new SqlParameter("@id", model.Id),
					new SqlParameter("@name",model.Name),
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

				return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageHeadermenu, ManageHeaderMenuAsync, output, newid: null, message, sqlParams.ToArray());

			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<BaseResponse<long>> Update(ManageHeaderMenu model)
		{
			try
			{
				var sqlParams = new List<SqlParameter>() {
					new SqlParameter("@mode", "update"),
					new SqlParameter("@id",model.Id),
					new SqlParameter("@name",model.Name),
					new SqlParameter("@image",model.Image),
					new SqlParameter("@imageAlt", model.ImageAlt),
					new SqlParameter("@hasLink", model.HasLink),
					new SqlParameter("@redirectTo", model.RedirectTo),
					new SqlParameter("@lendingPageId", model.LendingPageId),
					new SqlParameter("@categoryId", model.CategoryId),
					new SqlParameter("@staticPageId", model.StaticPageId),
					new SqlParameter("@collectionId", model.CollectionId),
					new SqlParameter("@customLink", model.CustomLink),
					new SqlParameter("@sequence", model.Sequence),
					new SqlParameter("@color", model.color),
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

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHeadermenu, output, newid, message, sqlParams.ToArray());

			}
			catch (Exception)
			{
				throw;
			}
		}

		private async Task<List<ManageHeaderMenu>> ManageHeaderMenuAsync(DbDataReader reader)
		{
			List<ManageHeaderMenu> manageHeaderMenu = new List<ManageHeaderMenu>();
			while (await reader.ReadAsync())
			{
				manageHeaderMenu.Add(new ManageHeaderMenu()
				{
					RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
					PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
					RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
					Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
					Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
					Image = Convert.ToString(reader.GetValue(reader.GetOrdinal("Image"))),
					ImageAlt = Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageAlt"))),
					HasLink = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("HasLink"))),
					RedirectTo = Convert.ToString(reader.GetValue(reader.GetOrdinal("RedirectTo"))),
					LendingPageId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LendingPageId"))),
					CategoryId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
					StaticPageId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("StaticPageId"))),
					CollectionId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CollectionId"))),
					CustomLink = Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomLink"))),
					Sequence = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Sequence"))),
					color = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("color")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("color"))),
					CreatedBy= Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
					CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
					ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
					ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt")))					
				});
			}
			return manageHeaderMenu;
		}
	}
}

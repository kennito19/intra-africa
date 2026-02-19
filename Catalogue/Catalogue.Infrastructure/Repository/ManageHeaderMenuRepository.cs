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
				var sqlParams = new List<MySqlParameter>()
				{
					new MySqlParameter("@mode","add"),
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
					new MySqlParameter("@sequence", model.Sequence),
					new MySqlParameter("@color", model.color),
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
				var sqlParams = new List<MySqlParameter>() {
					new MySqlParameter("@mode", Mode),
					new MySqlParameter("@id", model.Id),
					new MySqlParameter("@name",model.Name),
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
				var sqlParams = new List<MySqlParameter>() {
					new MySqlParameter("@mode", "update"),
					new MySqlParameter("@id",model.Id),
					new MySqlParameter("@name",model.Name),
					new MySqlParameter("@image",model.Image),
					new MySqlParameter("@imageAlt", model.ImageAlt),
					new MySqlParameter("@hasLink", model.HasLink),
					new MySqlParameter("@redirectTo", model.RedirectTo),
					new MySqlParameter("@lendingPageId", model.LendingPageId),
					new MySqlParameter("@categoryId", model.CategoryId),
					new MySqlParameter("@staticPageId", model.StaticPageId),
					new MySqlParameter("@collectionId", model.CollectionId),
					new MySqlParameter("@customLink", model.CustomLink),
					new MySqlParameter("@sequence", model.Sequence),
					new MySqlParameter("@color", model.color),
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

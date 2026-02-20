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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
	public class ManageOffersRepository : IManageOffersRepository
	{
		private readonly IConfiguration _configuration;
		private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
		MySqlConnection con;

		public ManageOffersRepository(IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DBconnection");
			con = new MySqlConnection(connectionString);

			_configuration = configuration;
		}
		public async Task<BaseResponse<long>> Create(ManageOffersLibrary model)
		{
			try
			{
				var sqlParams = new List<MySqlParameter>() {
				new MySqlParameter("@mode", "add"),
				new MySqlParameter("@name", model.name),
				new MySqlParameter("@code", model.code),
				new MySqlParameter("@terms", model.terms),
				new MySqlParameter("@offerType", model.offerType),
				new MySqlParameter("@offerCreateBy", model.offerCreatedBy),
				new MySqlParameter("@usesType", model.usesType),
				new MySqlParameter("@usesPerCustomer", model.usesPerCustomer),
				new MySqlParameter("@value", model.value),
				new MySqlParameter("@minimumOrderValue", model.minimumOrderValue),
				new MySqlParameter("@maximumDiscountAmount", model.maximumDiscountAmount),
				new MySqlParameter("@buyQty", model.buyQty),
				new MySqlParameter("@getQty", model.getQty),
				new MySqlParameter("@applyOn", model.applyOn),
				new MySqlParameter("@hasShippingFree", model.hasShippingFree),
				new MySqlParameter("@showToCustomer", model.showToCustomer),
				new MySqlParameter("@onlyForOnlinePayments", model.onlyForOnlinePayments),
				new MySqlParameter("@onlyForNewCustomers", model.onlyForNewCustomers),
				new MySqlParameter("@startDate", model.startDate),
				new MySqlParameter("@endDate", model.endDate),
				new MySqlParameter("@status", model.status),
				new MySqlParameter("@createdby", model.CreatedBy),
				new MySqlParameter("@createdat", model.CreatedAt),
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

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageOffers, output, newid, message, sqlParams.ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<BaseResponse<long>> Update(ManageOffersLibrary model)
		{
			try
			{
				var sqlParams = new List<MySqlParameter>() {
				new MySqlParameter("@mode", "update"),
				new MySqlParameter("@id", model.id),
				new MySqlParameter("@name", model.name),
				new MySqlParameter("@code", model.code),
				new MySqlParameter("@terms", model.terms),
				new MySqlParameter("@offerType", model.offerType),
				new MySqlParameter("@offerCreateBy", model.offerCreatedBy),
                new MySqlParameter("@usesType", model.usesType),
                new MySqlParameter("@usesPerCustomer", model.usesPerCustomer),
				new MySqlParameter("@value", model.value),
				new MySqlParameter("@minimumOrderValue", model.minimumOrderValue),
				new MySqlParameter("@maximumDiscountAmount", model.maximumDiscountAmount),
				new MySqlParameter("@buyQty", model.buyQty),
				new MySqlParameter("@getQty", model.getQty),
				new MySqlParameter("@applyOn", model.applyOn),
				new MySqlParameter("@hasShippingFree", model.hasShippingFree),
				new MySqlParameter("@showToCustomer", model.showToCustomer),
				new MySqlParameter("@onlyForOnlinePayments", model.onlyForOnlinePayments),
				new MySqlParameter("@onlyForNewCustomers", model.onlyForNewCustomers),
				new MySqlParameter("@startDate", model.startDate),
				new MySqlParameter("@endDate", model.endDate),
				new MySqlParameter("@status", model.status),
				new MySqlParameter("@modifiedby", model.ModifiedBy),
				new MySqlParameter("@modifiedat", model.ModifiedAt),
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

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageOffers, output, newid, message, sqlParams.ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<BaseResponse<long>> Delete(ManageOffersLibrary model)
		{
			try
			{
				var sqlParams = new List<MySqlParameter>() {
				new MySqlParameter("@mode", "delete"),
				new MySqlParameter("@id", model.id),
				new MySqlParameter("@deletedby", model.DeletedBy),
				new MySqlParameter("@deletedat", model.DeletedAt),
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

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageOffers, output, newid, message, sqlParams.ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<BaseResponse<List<ManageOffersLibrary>>> get(ManageOffersLibrary model, int PageIndex, int PageSize, string Mode)
		{
			try
			{
				var sqlParams = new List<MySqlParameter>() {
				new MySqlParameter("@mode", Mode),
				new MySqlParameter("@id", model.id),
				new MySqlParameter("@name", model.name),
				new MySqlParameter("@offerType", model.offerType),
				new MySqlParameter("@code", model.code),
				new MySqlParameter("@status", model.status),
				new MySqlParameter("@isdeleted", model.IsDeleted),
				new MySqlParameter("@hasShippingFree", model.hasShippingFree),
				new MySqlParameter("@offerids", model.offerIds),
				new MySqlParameter("@offerCreateBy", model.offerCreatedBy),
				new MySqlParameter("@createdby", model.CreatedBy),
				new MySqlParameter("@searchtext", model.Searchtext),
                new MySqlParameter("@showToCustomer", model.showToCustomer),
                new MySqlParameter("@pageIndex", PageIndex),
				new MySqlParameter("@PageSize", PageSize),
			};
				MySqlParameter output = new MySqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.MySqlDbType = MySqlDbType.Int32;

				//MySqlParameter newid = new MySqlParameter();
				//newid.ParameterName = "@newid";
				//newid.Direction = ParameterDirection.Output;
				//newid.MySqlDbType = MySqlDbType.Int64;

				MySqlParameter message = new MySqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.MySqlDbType = MySqlDbType.VarChar;
				message.Size = 50;

				return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageOffers, ManageOfferParserAsync, output, newid: null, message, sqlParams.ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private async Task<List<ManageOffersLibrary>> ManageOfferParserAsync(DbDataReader reader)
		{
			List<ManageOffersLibrary> offerList = new List<ManageOffersLibrary>();
			while (await reader.ReadAsync())
			{
				offerList.Add(new ManageOffersLibrary()
				{
					RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
					PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
					RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
					id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
					name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
					code = Convert.ToString(reader.GetValue(reader.GetOrdinal("Code"))),
					terms = Convert.ToString(reader.GetValue(reader.GetOrdinal("Terms"))),
					offerCreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("offerCreateBy"))),
					offerType = Convert.ToString(reader.GetValue(reader.GetOrdinal("offerType"))),
                    usesType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UsesType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UsesType"))),
                    usesPerCustomer = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UsesPerCustomer")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UsesPerCustomer"))),
                    value = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Value")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Value"))),
					minimumOrderValue = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("minimumOrderValue")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("minimumOrderValue"))),
					maximumDiscountAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("maximumDiscountAmount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("maximumDiscountAmount"))),
					buyQty = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("buyQty")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("buyQty"))),
					getQty = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("getQty")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("getQty"))),
					applyOn = Convert.ToString(reader.GetValue(reader.GetOrdinal("applyOn"))),
					hasShippingFree = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("hasShippingFree")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("hasShippingFree"))),
					showToCustomer = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("showToCustomer"))),
					onlyForOnlinePayments = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("onlyForOnlinePayments"))),
					onlyForNewCustomers = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("onlyForNewCustomers"))),
					startDate = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("startDate"))),
					endDate = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("endDate"))),
					status = Convert.ToString(reader.GetValue(reader.GetOrdinal("status"))),
					CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
					CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
					ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
					ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
					DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
					DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
					IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    OfferStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OfferStatus")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("OfferStatus"))),
				});
			}
			return offerList;
		}
	}
}

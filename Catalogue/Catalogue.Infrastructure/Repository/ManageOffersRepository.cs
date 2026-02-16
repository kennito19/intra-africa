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
		SqlConnection con;

		public ManageOffersRepository(IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DBconnection");
			con = new SqlConnection(connectionString);

			_configuration = configuration;
		}
		public async Task<BaseResponse<long>> Create(ManageOffersLibrary model)
		{
			try
			{
				var sqlParams = new List<SqlParameter>() {
				new SqlParameter("@mode", "add"),
				new SqlParameter("@name", model.name),
				new SqlParameter("@code", model.code),
				new SqlParameter("@terms", model.terms),
				new SqlParameter("@offerType", model.offerType),
				new SqlParameter("@offerCreateBy", model.offerCreatedBy),
				new SqlParameter("@usesType", model.usesType),
				new SqlParameter("@usesPerCustomer", model.usesPerCustomer),
				new SqlParameter("@value", model.value),
				new SqlParameter("@minimumOrderValue", model.minimumOrderValue),
				new SqlParameter("@maximumDiscountAmount", model.maximumDiscountAmount),
				new SqlParameter("@buyQty", model.buyQty),
				new SqlParameter("@getQty", model.getQty),
				new SqlParameter("@applyOn", model.applyOn),
				new SqlParameter("@hasShippingFree", model.hasShippingFree),
				new SqlParameter("@showToCustomer", model.showToCustomer),
				new SqlParameter("@onlyForOnlinePayments", model.onlyForOnlinePayments),
				new SqlParameter("@onlyForNewCustomers", model.onlyForNewCustomers),
				new SqlParameter("@startDate", model.startDate),
				new SqlParameter("@endDate", model.endDate),
				new SqlParameter("@status", model.status),
				new SqlParameter("@createdby", model.CreatedBy),
				new SqlParameter("@createdat", model.CreatedAt),
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
				var sqlParams = new List<SqlParameter>() {
				new SqlParameter("@mode", "update"),
				new SqlParameter("@id", model.id),
				new SqlParameter("@name", model.name),
				new SqlParameter("@code", model.code),
				new SqlParameter("@terms", model.terms),
				new SqlParameter("@offerType", model.offerType),
				new SqlParameter("@offerCreateBy", model.offerCreatedBy),
                new SqlParameter("@usesType", model.usesType),
                new SqlParameter("@usesPerCustomer", model.usesPerCustomer),
				new SqlParameter("@value", model.value),
				new SqlParameter("@minimumOrderValue", model.minimumOrderValue),
				new SqlParameter("@maximumDiscountAmount", model.maximumDiscountAmount),
				new SqlParameter("@buyQty", model.buyQty),
				new SqlParameter("@getQty", model.getQty),
				new SqlParameter("@applyOn", model.applyOn),
				new SqlParameter("@hasShippingFree", model.hasShippingFree),
				new SqlParameter("@showToCustomer", model.showToCustomer),
				new SqlParameter("@onlyForOnlinePayments", model.onlyForOnlinePayments),
				new SqlParameter("@onlyForNewCustomers", model.onlyForNewCustomers),
				new SqlParameter("@startDate", model.startDate),
				new SqlParameter("@endDate", model.endDate),
				new SqlParameter("@status", model.status),
				new SqlParameter("@modifiedby", model.ModifiedBy),
				new SqlParameter("@modifiedat", model.ModifiedAt),
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
				var sqlParams = new List<SqlParameter>() {
				new SqlParameter("@mode", "delete"),
				new SqlParameter("@id", model.id),
				new SqlParameter("@deletedby", model.DeletedBy),
				new SqlParameter("@deletedat", model.DeletedAt),
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
				var sqlParams = new List<SqlParameter>() {
				new SqlParameter("@mode", Mode),
				new SqlParameter("@id", model.id),
				new SqlParameter("@name", model.name),
				new SqlParameter("@offerType", model.offerType),
				new SqlParameter("@code", model.code),
				new SqlParameter("@status", model.status),
				new SqlParameter("@isdeleted", model.IsDeleted),
				new SqlParameter("@hasShippingFree", model.hasShippingFree),
				new SqlParameter("@offerids", model.offerIds),
				new SqlParameter("@offerCreateBy", model.offerCreatedBy),
				new SqlParameter("@createdby", model.CreatedBy),
				new SqlParameter("@searchtext", model.Searchtext),
                new SqlParameter("@showToCustomer", model.showToCustomer),
                new SqlParameter("@pageIndex", PageIndex),
				new SqlParameter("@PageSize", PageSize),
			};
				SqlParameter output = new SqlParameter();
				output.ParameterName = "@output";
				output.Direction = ParameterDirection.Output;
				output.SqlDbType = SqlDbType.Int;

				//SqlParameter newid = new SqlParameter();
				//newid.ParameterName = "@newid";
				//newid.Direction = ParameterDirection.Output;
				//newid.SqlDbType = SqlDbType.BigInt;

				SqlParameter message = new SqlParameter();
				message.ParameterName = "@message";
				message.Direction = ParameterDirection.Output;
				message.SqlDbType = SqlDbType.NVarChar;
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

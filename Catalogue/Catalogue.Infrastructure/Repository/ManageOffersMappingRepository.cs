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
	public class ManageOffersMappingRepository : IManageOffersMappingRepository
	{
		private readonly IConfiguration _configuration;
		private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
		SqlConnection con;

        public ManageOffersMappingRepository(IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DBconnection");
			con = new SqlConnection(connectionString);

			_configuration = configuration;
		}
		public async Task<BaseResponse<long>> Create(ManageOffersMappingLibrary model)
		{
			try
			{
				var sqlParams = new List<SqlParameter>() {
				new SqlParameter("@mode", "add"),
				new SqlParameter("@offerId", model.offerId),
				new SqlParameter("@categoryId", model.categoryId),
				new SqlParameter("@sellerId", model.sellerId),
				new SqlParameter("@brandId", model.brandId),
				new SqlParameter("@productId", model.productId),
				new SqlParameter("@getProductId", model.getProductId),
				new SqlParameter("@userId", model.userId),
				new SqlParameter("@getDiscountType", model.getDiscountType),
				new SqlParameter("@getDiscountValue", model.getDiscountValue),
				new SqlParameter("@getProductPrice", model.getProductPrice),
				new SqlParameter("@sellerOptIn", model.sellerOptIn),
				new SqlParameter("@optInSellerIds", model.optInSellerIds),
				new SqlParameter("@status", model.status),
                new SqlParameter("@categoryIds", model.CategoryIds),
                new SqlParameter("@sellerIds", model.SellerIds),
                new SqlParameter("@brandIds", model.Brandids),
                new SqlParameter("@productIds", model.ProductIds),
                new SqlParameter("@extraDetails", model.ExtraDetails),
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

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageOffersMapping, output, newid, message, sqlParams.ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<BaseResponse<long>> Update(ManageOffersMappingLibrary model)
		{
			try
			{
				var sqlParams = new List<SqlParameter>() {
				new SqlParameter("@mode", "update"),
				new SqlParameter("@id", model.id),
				new SqlParameter("@offerId", model.offerId),
				new SqlParameter("@categoryId", model.categoryId),
				new SqlParameter("@sellerId", model.sellerId),
				new SqlParameter("@brandId", model.brandId),
				new SqlParameter("@productId", model.productId),
				new SqlParameter("@getProductId", model.getProductId),
				new SqlParameter("@userId", model.userId),
				new SqlParameter("@getDiscountType", model.getDiscountType),
				new SqlParameter("@getDiscountValue", model.getDiscountValue),
				new SqlParameter("@getProductPrice", model.getProductPrice),
				new SqlParameter("@sellerOptIn", model.sellerOptIn),
				new SqlParameter("@optInSellerIds", model.optInSellerIds),
				new SqlParameter("@status", model.status),
                new SqlParameter("@categoryIds", model.CategoryIds),
                new SqlParameter("@sellerIds", model.SellerIds),
                new SqlParameter("@brandIds", model.Brandids),
                new SqlParameter("@productIds", model.ProductIds),
                new SqlParameter("@extraDetails", model.ExtraDetails),
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

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageOffersMapping, output, newid, message, sqlParams.ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<BaseResponse<long>> Delete(ManageOffersMappingLibrary model)
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

				return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageOffersMapping, output, newid, message, sqlParams.ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<BaseResponse<List<ManageOffersMappingLibrary>>> get(ManageOffersMappingLibrary model, int PageIndex, int PageSize, string Mode)
		{
			try
			{
				var sqlParams = new List<SqlParameter>() {
				new SqlParameter("@mode", Mode),
				new SqlParameter("@id", model.id),
				new SqlParameter("@offerId", model.offerIds),
				new SqlParameter("@categoryId", model.categoryId),
				new SqlParameter("@brandId", model.brandId),
				new SqlParameter("@productId", model.productId),
				new SqlParameter("@getProductId", model.getProductId),
				new SqlParameter("@sellerId", model.sellerId),
				new SqlParameter("@userId", model.userId),
				new SqlParameter("@offerName", model.offerName),
				new SqlParameter("@productName", model.productName),
				new SqlParameter("@categoryName", model.categoryName),
				new SqlParameter("@status", model.status),
				new SqlParameter("@IsDeleted", model.IsDeleted),
				new SqlParameter("@sellerOptIn", model.sellerOptIn),
				new SqlParameter("@searchtext", model.Searchtext),
                new SqlParameter("@applyOn", model.ApplyOn),
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

				return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageOffersMapping, ManageOfferMappingParserAsync, output, newid: null, message, sqlParams.ToArray());
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private async Task<List<ManageOffersMappingLibrary>> ManageOfferMappingParserAsync(DbDataReader reader)
		{
			List<ManageOffersMappingLibrary> offersMappingList = new List<ManageOffersMappingLibrary>();
			while (await reader.ReadAsync())
			{
				offersMappingList.Add(new ManageOffersMappingLibrary()
				{
					RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
					PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
					RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
					id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
					offerId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("offerId"))),
					categoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("categoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("categoryId"))),
					sellerId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("sellerId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("sellerId"))),
					brandId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("brandId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("brandId"))),
					productId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("productId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("productId"))),
					getProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("getProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("getProductId"))),
					userId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("userId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("userId"))),
					getDiscountType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("getDiscountType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("getDiscountType"))),
					getDiscountValue = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("getDiscountValue")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("getDiscountValue"))),
					getProductPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("getProductPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("getProductPrice"))),
					sellerOptIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("sellerOptIn")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("sellerOptIn"))),
					optInSellerIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("optInSellerIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("optInSellerIds"))),
					status = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("status")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("status"))),
					ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    CategoryIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryIds"))),
                    SellerIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerIds"))),
                    Brandids = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Brandids")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Brandids"))),
                    ProductIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductIds"))),
					CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
					CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
					ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
					ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
					DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
					DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
					IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
					offerName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OfferName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OfferName"))),
					categoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    categoryPathNames = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames"))),
					productName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
				});
			}
			return offersMappingList;
		}
	}
}

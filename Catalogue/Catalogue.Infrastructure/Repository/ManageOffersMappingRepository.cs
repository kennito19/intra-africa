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
	public class ManageOffersMappingRepository : IManageOffersMappingRepository
	{
		private readonly IConfiguration _configuration;
		private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
		MySqlConnection con;

        public ManageOffersMappingRepository(IConfiguration configuration)
		{
			string connectionString = configuration.GetConnectionString("DBconnection");
			con = new MySqlConnection(connectionString);

			_configuration = configuration;
		}
		public async Task<BaseResponse<long>> Create(ManageOffersMappingLibrary model)
		{
			try
			{
				var sqlParams = new List<MySqlParameter>() {
				new MySqlParameter("@mode", "add"),
				new MySqlParameter("@offerId", model.offerId),
				new MySqlParameter("@categoryId", model.categoryId),
				new MySqlParameter("@sellerId", model.sellerId),
				new MySqlParameter("@brandId", model.brandId),
				new MySqlParameter("@productId", model.productId),
				new MySqlParameter("@getProductId", model.getProductId),
				new MySqlParameter("@userId", model.userId),
				new MySqlParameter("@getDiscountType", model.getDiscountType),
				new MySqlParameter("@getDiscountValue", model.getDiscountValue),
				new MySqlParameter("@getProductPrice", model.getProductPrice),
				new MySqlParameter("@sellerOptIn", model.sellerOptIn),
				new MySqlParameter("@optInSellerIds", model.optInSellerIds),
				new MySqlParameter("@status", model.status),
                new MySqlParameter("@categoryIds", model.CategoryIds),
                new MySqlParameter("@sellerIds", model.SellerIds),
                new MySqlParameter("@brandIds", model.Brandids),
                new MySqlParameter("@productIds", model.ProductIds),
                new MySqlParameter("@extraDetails", model.ExtraDetails),
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
				var sqlParams = new List<MySqlParameter>() {
				new MySqlParameter("@mode", "update"),
				new MySqlParameter("@id", model.id),
				new MySqlParameter("@offerId", model.offerId),
				new MySqlParameter("@categoryId", model.categoryId),
				new MySqlParameter("@sellerId", model.sellerId),
				new MySqlParameter("@brandId", model.brandId),
				new MySqlParameter("@productId", model.productId),
				new MySqlParameter("@getProductId", model.getProductId),
				new MySqlParameter("@userId", model.userId),
				new MySqlParameter("@getDiscountType", model.getDiscountType),
				new MySqlParameter("@getDiscountValue", model.getDiscountValue),
				new MySqlParameter("@getProductPrice", model.getProductPrice),
				new MySqlParameter("@sellerOptIn", model.sellerOptIn),
				new MySqlParameter("@optInSellerIds", model.optInSellerIds),
				new MySqlParameter("@status", model.status),
                new MySqlParameter("@categoryIds", model.CategoryIds),
                new MySqlParameter("@sellerIds", model.SellerIds),
                new MySqlParameter("@brandIds", model.Brandids),
                new MySqlParameter("@productIds", model.ProductIds),
                new MySqlParameter("@extraDetails", model.ExtraDetails),
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
				var sqlParams = new List<MySqlParameter>() {
				new MySqlParameter("@mode", Mode),
				new MySqlParameter("@id", model.id),
				new MySqlParameter("@offerId", model.offerIds),
				new MySqlParameter("@categoryId", model.categoryId),
				new MySqlParameter("@brandId", model.brandId),
				new MySqlParameter("@productId", model.productId),
				new MySqlParameter("@getProductId", model.getProductId),
				new MySqlParameter("@sellerId", model.sellerId),
				new MySqlParameter("@userId", model.userId),
				new MySqlParameter("@offerName", model.offerName),
				new MySqlParameter("@productName", model.productName),
				new MySqlParameter("@categoryName", model.categoryName),
				new MySqlParameter("@status", model.status),
				new MySqlParameter("@IsDeleted", model.IsDeleted),
				new MySqlParameter("@sellerOptIn", model.sellerOptIn),
				new MySqlParameter("@searchtext", model.Searchtext),
                new MySqlParameter("@applyOn", model.ApplyOn),
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

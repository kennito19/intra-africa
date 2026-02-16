using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Domain;
using User.Domain.Entity;
using User.Infrastructure.Helper;

namespace User.Infrastructure.Repository
{
    public class AssignBrandToSellerRepository : IAssignBrandToSellerRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public AssignBrandToSellerRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(AssignBrandToSeller assignBrandToSeller)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","add"),
                    new SqlParameter("@id",assignBrandToSeller.Id),
                    new SqlParameter("@sellerid",assignBrandToSeller.SellerID),
                    new SqlParameter("@brandid",assignBrandToSeller.BrandId),
                    new SqlParameter("@status",assignBrandToSeller.Status),
                    new SqlParameter("@createdBy", assignBrandToSeller.CreatedBy),
                    new SqlParameter("@createdAt", assignBrandToSeller.CreatedAt),
                    new SqlParameter("@brandcerti",assignBrandToSeller.BrandCertificate)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignBrandToSeller, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<long>> Update(AssignBrandToSeller assignBrandToSeller)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id",assignBrandToSeller.Id),
                    new SqlParameter("@sellerid",assignBrandToSeller.SellerID),
                    new SqlParameter("@brandid",assignBrandToSeller.BrandId),
                    new SqlParameter("@status",assignBrandToSeller.Status),
                    new SqlParameter("@createdBy", assignBrandToSeller.CreatedBy),
                    new SqlParameter("@createdAt", assignBrandToSeller.CreatedAt),
                    new SqlParameter("@modifiedBy", assignBrandToSeller.ModifiedBy),
                    new SqlParameter("@modifiedAt", assignBrandToSeller.ModifiedAt),
                    new SqlParameter("@brandcerti",assignBrandToSeller.BrandCertificate),
                    new SqlParameter("@isDeleted", assignBrandToSeller.IsDeleted),
                    new SqlParameter("@deletedBy", assignBrandToSeller.DeletedBy),
                    new SqlParameter("@deletedAt", assignBrandToSeller.DeletedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignBrandToSeller, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Delete(AssignBrandToSeller assignBrandToSeller)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", assignBrandToSeller.Id),
                    new SqlParameter("@deletedBy", assignBrandToSeller.DeletedBy),
                    new SqlParameter("@deletedAt", assignBrandToSeller.DeletedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignBrandToSeller, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<AssignBrandToSeller>>> Get(AssignBrandToSeller assignBrandToSeller, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", assignBrandToSeller.Id),
                    new SqlParameter("@sellerid", assignBrandToSeller.SellerID),
                    new SqlParameter("@brandid", assignBrandToSeller.BrandId),
                    new SqlParameter("@sellername", assignBrandToSeller.SellerName),
                    new SqlParameter("@brandname", assignBrandToSeller.BrandName),
                    new SqlParameter("@status", assignBrandToSeller.Status),
                    new SqlParameter("@Brandstatus", assignBrandToSeller.BrandStatus),
                    new SqlParameter("@isDeleted", assignBrandToSeller.IsDeleted),
                    new SqlParameter("@isBrandDeleted", assignBrandToSeller.IsBrandDeleted),
                    new SqlParameter("@pageIndex", PageIndex),
                    new SqlParameter("@pageSize", PageSize),
                    new SqlParameter("@searchtext", assignBrandToSeller.Searchtext),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetAssignBrandToSeller, AssignBrandToSellerParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }


        private async Task<List<AssignBrandToSeller>> AssignBrandToSellerParserAsync(DbDataReader reader)
        {
            List<AssignBrandToSeller> lstAssignBrandToSeller = new List<AssignBrandToSeller>();
            while (await reader.ReadAsync())
            {
                lstAssignBrandToSeller.Add(new AssignBrandToSeller()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    SellerID = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    SellerName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerName"))),
                    BrandId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandId"))),
                    BrandName = Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),



                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    BrandCertificate = Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandCertificate"))),
                    Logo = Convert.ToString(reader.GetValue(reader.GetOrdinal("Logo"))),
                    BrandStatus = Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandStatus"))),
                    BrandGUID = Convert.ToString(reader.GetValue(reader.GetOrdinal("GUID"))),
                    BrandDescription = Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),

                });
            }
            return lstAssignBrandToSeller;
        }
    }
}

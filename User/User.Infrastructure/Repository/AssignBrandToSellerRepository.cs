using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
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
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                    new MySqlParameter("@id",assignBrandToSeller.Id),
                    new MySqlParameter("@sellerid",assignBrandToSeller.SellerID),
                    new MySqlParameter("@brandid",assignBrandToSeller.BrandId),
                    new MySqlParameter("@status",assignBrandToSeller.Status),
                    new MySqlParameter("@createdBy", assignBrandToSeller.CreatedBy),
                    new MySqlParameter("@createdAt", assignBrandToSeller.CreatedAt),
                    new MySqlParameter("@brandcerti",assignBrandToSeller.BrandCertificate)
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id",assignBrandToSeller.Id),
                    new MySqlParameter("@sellerid",assignBrandToSeller.SellerID),
                    new MySqlParameter("@brandid",assignBrandToSeller.BrandId),
                    new MySqlParameter("@status",assignBrandToSeller.Status),
                    new MySqlParameter("@createdBy", assignBrandToSeller.CreatedBy),
                    new MySqlParameter("@createdAt", assignBrandToSeller.CreatedAt),
                    new MySqlParameter("@modifiedBy", assignBrandToSeller.ModifiedBy),
                    new MySqlParameter("@modifiedAt", assignBrandToSeller.ModifiedAt),
                    new MySqlParameter("@brandcerti",assignBrandToSeller.BrandCertificate),
                    new MySqlParameter("@isDeleted", assignBrandToSeller.IsDeleted),
                    new MySqlParameter("@deletedBy", assignBrandToSeller.DeletedBy),
                    new MySqlParameter("@deletedAt", assignBrandToSeller.DeletedAt)

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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", assignBrandToSeller.Id),
                    new MySqlParameter("@deletedBy", assignBrandToSeller.DeletedBy),
                    new MySqlParameter("@deletedAt", assignBrandToSeller.DeletedAt)
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", assignBrandToSeller.Id),
                    new MySqlParameter("@sellerid", assignBrandToSeller.SellerID),
                    new MySqlParameter("@brandid", assignBrandToSeller.BrandId),
                    new MySqlParameter("@sellername", assignBrandToSeller.SellerName),
                    new MySqlParameter("@brandname", assignBrandToSeller.BrandName),
                    new MySqlParameter("@status", assignBrandToSeller.Status),
                    new MySqlParameter("@Brandstatus", assignBrandToSeller.BrandStatus),
                    new MySqlParameter("@isDeleted", assignBrandToSeller.IsDeleted),
                    new MySqlParameter("@isBrandDeleted", assignBrandToSeller.IsBrandDeleted),
                    new MySqlParameter("@pageIndex", PageIndex),
                    new MySqlParameter("@pageSize", PageSize),
                    new MySqlParameter("@searchtext", assignBrandToSeller.Searchtext),
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

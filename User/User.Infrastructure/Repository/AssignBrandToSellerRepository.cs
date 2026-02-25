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
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public AssignBrandToSellerRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

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
                await using var connection = new MySqlConnection(_configuration.GetConnectionString("DBconnection"));
                await connection.OpenAsync();
                await using var cmd = new MySqlCommand();
                cmd.Connection = connection;

                var where = new List<string>();
                if (assignBrandToSeller.Id > 0)
                {
                    where.Add("a.Id = @id");
                    cmd.Parameters.AddWithValue("@id", assignBrandToSeller.Id);
                }
                if (!string.IsNullOrWhiteSpace(assignBrandToSeller.SellerID))
                {
                    where.Add("a.SellerID = @sellerId");
                    cmd.Parameters.AddWithValue("@sellerId", assignBrandToSeller.SellerID);
                }
                if (assignBrandToSeller.BrandId > 0)
                {
                    where.Add("a.BrandId = @brandId");
                    cmd.Parameters.AddWithValue("@brandId", assignBrandToSeller.BrandId);
                }
                if (!string.IsNullOrWhiteSpace(assignBrandToSeller.Status))
                {
                    where.Add("a.Status = @status");
                    cmd.Parameters.AddWithValue("@status", assignBrandToSeller.Status);
                }
                if (!string.IsNullOrWhiteSpace(assignBrandToSeller.Searchtext))
                {
                    where.Add("(b.Name LIKE @search OR a.SellerID LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{assignBrandToSeller.Searchtext}%");
                }
                where.Add("COALESCE(a.IsDeleted,0) = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", assignBrandToSeller.IsDeleted);

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM AssignBrandToSeller a
LEFT JOIN Brand b ON b.ID = a.BrandId
{whereClause};";
                var totalObj = await cmd.ExecuteScalarAsync();
                var total = Convert.ToInt32(totalObj ?? 0);

                var result = new List<AssignBrandToSeller>();
                if (total > 0)
                {
                    var fetchAll = PageIndex <= 0 || PageSize <= 0;
                    var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                    var safePageSize = PageSize <= 0 ? total : PageSize;
                    var offset = (safePageIndex - 1) * safePageSize;

                    cmd.CommandText = $@"
SELECT
    a.Id, a.SellerID, a.BrandId, a.Status, a.CreatedBy, a.CreatedAt, a.ModifiedBy, a.ModifiedAt,
    a.DeletedBy, a.DeletedAt, a.IsDeleted, a.BrandCertificate,
    b.Name AS BrandName, b.Logo, b.Status AS BrandStatus, b.GUID, b.Description
FROM AssignBrandToSeller a
LEFT JOIN Brand b ON b.ID = a.BrandId
{whereClause}
ORDER BY a.Id DESC
{(fetchAll ? string.Empty : "LIMIT @offset, @pageSize")};";

                    if (!fetchAll)
                    {
                        cmd.Parameters.AddWithValue("@offset", offset);
                        cmd.Parameters.AddWithValue("@pageSize", safePageSize);
                    }

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNo = 0;
                    var pageCount = safePageSize == 0 ? 1 : (int)Math.Ceiling(total / (double)safePageSize);
                    while (await reader.ReadAsync())
                    {
                        rowNo++;
                        result.Add(new AssignBrandToSeller
                        {
                            RowNumber = rowNo,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32("Id"),
                            SellerID = reader.IsDBNull("SellerID") ? null : reader.GetString("SellerID"),
                            BrandId = reader.IsDBNull("BrandId") ? 0 : reader.GetInt32("BrandId"),
                            BrandName = reader.IsDBNull("BrandName") ? null : reader.GetString("BrandName"),
                            Status = reader.IsDBNull("Status") ? null : reader.GetString("Status"),
                            CreatedBy = reader.IsDBNull("CreatedBy") ? null : reader.GetString("CreatedBy"),
                            CreatedAt = reader.IsDBNull("CreatedAt") ? DateTime.MinValue : reader.GetDateTime("CreatedAt"),
                            ModifiedBy = reader.IsDBNull("ModifiedBy") ? null : reader.GetString("ModifiedBy"),
                            ModifiedAt = reader.IsDBNull("ModifiedAt") ? null : reader.GetDateTime("ModifiedAt"),
                            DeletedBy = reader.IsDBNull("DeletedBy") ? null : reader.GetString("DeletedBy"),
                            DeletedAt = reader.IsDBNull("DeletedAt") ? null : reader.GetDateTime("DeletedAt"),
                            IsDeleted = !reader.IsDBNull("IsDeleted") && reader.GetBoolean("IsDeleted"),
                            BrandCertificate = reader.IsDBNull("BrandCertificate") ? null : reader.GetString("BrandCertificate"),
                            Logo = reader.IsDBNull("Logo") ? null : reader.GetString("Logo"),
                            BrandStatus = reader.IsDBNull("BrandStatus") ? null : reader.GetString("BrandStatus"),
                            BrandGUID = reader.IsDBNull("GUID") ? null : reader.GetString("GUID"),
                            BrandDescription = reader.IsDBNull("Description") ? null : reader.GetString("Description")
                        });
                    }
                }

                return new BaseResponse<List<AssignBrandToSeller>>
                {
                    code = result.Count > 0 ? 200 : 204,
                    message = result.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AssignBrandToSeller>>
                {
                    code = 400,
                    message = ex.Message,
                    data = new List<AssignBrandToSeller>()
                };
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

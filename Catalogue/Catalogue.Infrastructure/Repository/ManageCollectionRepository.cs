using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Catalogue.Infrastructure.Repository
{
    public class ManageCollectionRepository : IManageCollectionRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public ManageCollectionRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ManageCollectionLibrary collection)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@name", collection.Name),
                    new SqlParameter("@type", collection.Type),
                    new SqlParameter("@image", collection.Image),
                    new SqlParameter("@title", collection.Title),
                    new SqlParameter("@description", collection.Description),
                    new SqlParameter("@subTitle", collection.SubTitle),
                    new SqlParameter("@startdate", collection.StartDate),
                    new SqlParameter("@enddate", collection.EndDate),
                    new SqlParameter("@status", collection.Status),
                    new SqlParameter("@sequence", collection.Sequence),
                    new SqlParameter("@createdby", collection.CreatedBy),
                new SqlParameter("@createdat", collection.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageCollection, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(ManageCollectionLibrary collection)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", collection.Id),
                new SqlParameter("@name", collection.Name),
                new SqlParameter("@type", collection.Type),
                new SqlParameter("@image", collection.Image),
                new SqlParameter("@title", collection.Title),
                new SqlParameter("@description", collection.Description),
                new SqlParameter("@subTitle", collection.SubTitle),
                new SqlParameter("@startdate", collection.StartDate),
                new SqlParameter("@enddate", collection.EndDate),
                new SqlParameter("@status", collection.Status),
                new SqlParameter("@sequence", collection.Sequence),
                new SqlParameter("@modifiedby", collection.ModifiedBy),
                new SqlParameter("@modifiedat", collection.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageCollection, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageCollectionLibrary collection)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", collection.Id),
                new SqlParameter("@deletedby", collection.DeletedBy),
                new SqlParameter("@deletedat", collection.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageCollection, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ManageCollectionLibrary>>> get(ManageCollectionLibrary collection, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", collection.Id),
                new SqlParameter("@name", collection.Name),
                new SqlParameter("@type", collection.Type),
                new SqlParameter("@title", collection.Title),
                new SqlParameter("@status", collection.Status),
                new SqlParameter("@date", collection.date),
                new SqlParameter("@isdeleted", collection.IsDeleted),
                new SqlParameter("@searchtext", collection.SearchText),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageCollection, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ManageCollectionLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<ManageCollectionLibrary> collection = new List<ManageCollectionLibrary>();
            while (await reader.ReadAsync())
            {
                collection.Add(new ManageCollectionLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    Type = Convert.ToString(reader.GetValue(reader.GetOrdinal("Type"))),
                    Image = Convert.ToString(reader.GetValue(reader.GetOrdinal("Image"))),
                    Title = Convert.ToString(reader.GetValue(reader.GetOrdinal("Title"))),
                    Description = Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    SubTitle = Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle"))),
                    StartDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("StartDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("StartDate"))),
                    EndDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("EndDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("EndDate"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    Sequence = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Sequence"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    TotalProducts = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalProducts")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalProducts"))),
                });
            }
            return collection;
        }
    }
}

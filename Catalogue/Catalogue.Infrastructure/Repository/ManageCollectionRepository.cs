using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
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
        MySqlConnection con;

        public ManageCollectionRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ManageCollectionLibrary collection)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@name", collection.Name),
                    new MySqlParameter("@type", collection.Type),
                    new MySqlParameter("@image", collection.Image),
                    new MySqlParameter("@title", collection.Title),
                    new MySqlParameter("@description", collection.Description),
                    new MySqlParameter("@subTitle", collection.SubTitle),
                    new MySqlParameter("@startdate", collection.StartDate),
                    new MySqlParameter("@enddate", collection.EndDate),
                    new MySqlParameter("@status", collection.Status),
                    new MySqlParameter("@sequence", collection.Sequence),
                    new MySqlParameter("@createdby", collection.CreatedBy),
                new MySqlParameter("@createdat", collection.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", collection.Id),
                new MySqlParameter("@name", collection.Name),
                new MySqlParameter("@type", collection.Type),
                new MySqlParameter("@image", collection.Image),
                new MySqlParameter("@title", collection.Title),
                new MySqlParameter("@description", collection.Description),
                new MySqlParameter("@subTitle", collection.SubTitle),
                new MySqlParameter("@startdate", collection.StartDate),
                new MySqlParameter("@enddate", collection.EndDate),
                new MySqlParameter("@status", collection.Status),
                new MySqlParameter("@sequence", collection.Sequence),
                new MySqlParameter("@modifiedby", collection.ModifiedBy),
                new MySqlParameter("@modifiedat", collection.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", collection.Id),
                new MySqlParameter("@deletedby", collection.DeletedBy),
                new MySqlParameter("@deletedat", collection.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", collection.Id),
                new MySqlParameter("@name", collection.Name),
                new MySqlParameter("@type", collection.Type),
                new MySqlParameter("@title", collection.Title),
                new MySqlParameter("@status", collection.Status),
                new MySqlParameter("@date", collection.date),
                new MySqlParameter("@isdeleted", collection.IsDeleted),
                new MySqlParameter("@searchtext", collection.SearchText),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize),
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

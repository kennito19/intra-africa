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
    //public class ProductsVideoLinksRepository : IProductsVideoLinksRepository
    //{
    //    private readonly IConfiguration _configuration;
    //    private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
    //    SqlConnection con;

    //    public ProductsVideoLinksRepository(IConfiguration configuration)
    //    {

    //        string connectionString = configuration.GetConnectionString("DBconnection");
    //        con = new SqlConnection(connectionString);

    //        _configuration = configuration;
    //    }
    //    public async Task<BaseResponse<long>> Create(ProductVideoLinks productVideoLinks)
    //    {
    //        try
    //        {
    //            var sqlParams = new List<SqlParameter>() {
    //            new SqlParameter("@mode", "add"),
    //            new SqlParameter("@productid", productVideoLinks.ProductID),
    //            new SqlParameter("@link", productVideoLinks.Link),
    //            new SqlParameter("@createdby", productVideoLinks.CreatedBy),
    //            new SqlParameter("@createdat", productVideoLinks.CreatedAt),
    //        };

    //            SqlParameter output = new SqlParameter();
    //            output.ParameterName = "@output";
    //            output.Direction = ParameterDirection.Output;
    //            output.SqlDbType = SqlDbType.Int;

    //            SqlParameter newid = new SqlParameter();
    //            newid.ParameterName = "@newid";
    //            newid.Direction = ParameterDirection.Output;
    //            newid.SqlDbType = SqlDbType.BigInt;

    //            SqlParameter message = new SqlParameter();
    //            message.ParameterName = "@message";
    //            message.Direction = ParameterDirection.Output;
    //            message.SqlDbType = SqlDbType.NVarChar;
    //            message.Size = 50;

    //            return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductVideoLinks, output, newid, message, sqlParams.ToArray());
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }
    //    public async Task<BaseResponse<long>> Update(ProductVideoLinks productVideoLinks)
    //    {
    //        try
    //        {
    //            var sqlParams = new List<SqlParameter>() {
    //            new SqlParameter("@mode", "update"),
    //            new SqlParameter("@id", productVideoLinks.Id),
    //            new SqlParameter("@link", productVideoLinks.Link),
    //            new SqlParameter("@modifiedby", productVideoLinks.ModifiedBy),
    //            new SqlParameter("@modifiedat", productVideoLinks.ModifiedAt),
    //        };

    //            SqlParameter output = new SqlParameter();
    //            output.ParameterName = "@output";
    //            output.Direction = ParameterDirection.Output;
    //            output.SqlDbType = SqlDbType.Int;

    //            SqlParameter newid = new SqlParameter();
    //            newid.ParameterName = "@newid";
    //            newid.Direction = ParameterDirection.Output;
    //            newid.SqlDbType = SqlDbType.BigInt;

    //            SqlParameter message = new SqlParameter();
    //            message.ParameterName = "@message";
    //            message.Direction = ParameterDirection.Output;
    //            message.SqlDbType = SqlDbType.NVarChar;
    //            message.Size = 50;

    //            return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductVideoLinks, output, newid, message, sqlParams.ToArray());
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }
    //    public async Task<BaseResponse<long>> Delete(ProductVideoLinks productVideoLinks)
    //    {
    //        try
    //        {
    //            var sqlParams = new List<SqlParameter>() {
    //            new SqlParameter("@mode", "delete"),
    //            new SqlParameter("@productid", productVideoLinks.ProductID),
    //        };

    //            SqlParameter output = new SqlParameter();
    //            output.ParameterName = "@output";
    //            output.Direction = ParameterDirection.Output;
    //            output.SqlDbType = SqlDbType.Int;

    //            SqlParameter newid = new SqlParameter();
    //            newid.ParameterName = "@newid";
    //            newid.Direction = ParameterDirection.Output;
    //            newid.SqlDbType = SqlDbType.BigInt;

    //            SqlParameter message = new SqlParameter();
    //            message.ParameterName = "@message";
    //            message.Direction = ParameterDirection.Output;
    //            message.SqlDbType = SqlDbType.NVarChar;
    //            message.Size = 50;

    //            return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductVideoLinks, output, newid, message, sqlParams.ToArray());
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    public async Task<BaseResponse<List<ProductVideoLinks>>> get(ProductVideoLinks productVideoLinks, int PageIndex, int PageSize, string Mode)
    //    {
    //        try
    //        {
    //            var sqlParams = new List<SqlParameter>() {
    //            //new SqlParameter("@mode", "get"),
    //            new SqlParameter("@mode", Mode),
    //            new SqlParameter("@id", productVideoLinks.Id),
    //            new SqlParameter("@productid", productVideoLinks.ProductID),
    //            new SqlParameter("@pageIndex", PageIndex),
    //            new SqlParameter("@PageSize", PageSize),

    //        };
    //            SqlParameter output = new SqlParameter();
    //            output.ParameterName = "@output";
    //            output.Direction = ParameterDirection.Output;
    //            output.SqlDbType = SqlDbType.Int;

    //            //SqlParameter newid = new SqlParameter();
    //            //newid.ParameterName = "@newid";
    //            //newid.Direction = ParameterDirection.Output;
    //            //newid.SqlDbType = SqlDbType.BigInt;

    //            SqlParameter message = new SqlParameter();
    //            message.ParameterName = "@message";
    //            message.Direction = ParameterDirection.Output;
    //            message.SqlDbType = SqlDbType.NVarChar;
    //            message.Size = 50;

    //            return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductVideoLinks, productVideoLinksParserAsync, output, newid: null, message, sqlParams.ToArray());
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message);
    //        }
    //    }

    //    private async Task<List<ProductVideoLinks>> productVideoLinksParserAsync(DbDataReader reader)
    //    {
    //        List<ProductVideoLinks> lstproductVideoLink = new List<ProductVideoLinks>();
    //        while (await reader.ReadAsync())
    //        {
    //            lstproductVideoLink.Add(new ProductVideoLinks()
    //            {
    //                RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
    //                PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
    //                RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
    //                Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
    //                ProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
    //                Link = Convert.ToString(reader.GetValue(reader.GetOrdinal("Link"))),
    //                CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
    //                CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
    //                ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
    //                ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
    //            });
    //        }
    //        return lstproductVideoLink;
    //    }

    //}
}

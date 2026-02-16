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
    public class ManageLendingPageSectionsRepository : IManageLendingPageSectionsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public ManageLendingPageSectionsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(LendingPageSectionsLibrary lendingPageSection)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@lendingPageId", lendingPageSection.LendingPageId),
                    new SqlParameter("@layoutId", lendingPageSection.LayoutId),
                    new SqlParameter("@layouttypeId", lendingPageSection.LayoutTypeId),
                    new SqlParameter("@name", lendingPageSection.Name),
                    new SqlParameter("@sequence", lendingPageSection.Sequence),
                    new SqlParameter("@sectionColumns", lendingPageSection.SectionColumns),
                    new SqlParameter("@title", lendingPageSection.Title),
                    new SqlParameter("@subTitle", lendingPageSection.SubTitle),
                    new SqlParameter("@linktext", lendingPageSection.LinkText),
                    new SqlParameter("@link", lendingPageSection.Link),
                    new SqlParameter("@status", lendingPageSection.Status),
                    new SqlParameter("@listtype", lendingPageSection.ListType),
                    new SqlParameter("@topproducts", lendingPageSection.TopProducts),
                    new SqlParameter("@categoryid", lendingPageSection.CategoryId),
                    new SqlParameter("@isTitleVisible", lendingPageSection.IsTitleVisible),
                    new SqlParameter("@titlePosition", lendingPageSection.TitlePosition),
                    new SqlParameter("@linkin", lendingPageSection.LinkIn),
                    new SqlParameter("@linkPosition", lendingPageSection.LinkPosition),
                    new SqlParameter("@backgroundColor", lendingPageSection.BackgroundColor),
                    new SqlParameter("@incontainer", lendingPageSection.InContainer),
                    new SqlParameter("@titleColor", lendingPageSection.TitleColor),
                    new SqlParameter("@textColor", lendingPageSection.TextColor),
                    new SqlParameter("@totalRowsInSection", lendingPageSection.TotalRowsInSection),
                    new SqlParameter("@isCustomGrid", lendingPageSection.IsCustomGrid),
                    new SqlParameter("@numberOfImages", lendingPageSection.NumberOfImages),
                    new SqlParameter("@column1", lendingPageSection.Column1),
                    new SqlParameter("@column2", lendingPageSection.Column2),
                    new SqlParameter("@column3", lendingPageSection.Column3),
                    new SqlParameter("@column4", lendingPageSection.Column4),
                    new SqlParameter("@createdby", lendingPageSection.CreatedBy),
                    new SqlParameter("@createdat", lendingPageSection.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.LendingPageSections, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(LendingPageSectionsLibrary lendingPageSection)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", lendingPageSection.Id),
                new SqlParameter("@lendingPageId", lendingPageSection.LendingPageId),
                new SqlParameter("@layoutId", lendingPageSection.LayoutId),
                new SqlParameter("@layouttypeId", lendingPageSection.LayoutTypeId),
                new SqlParameter("@name", lendingPageSection.Name),
                new SqlParameter("@sequence", lendingPageSection.Sequence),
                new SqlParameter("@sectionColumns", lendingPageSection.SectionColumns),
                new SqlParameter("@title", lendingPageSection.Title),
                new SqlParameter("@subTitle", lendingPageSection.SubTitle),
                new SqlParameter("@linktext", lendingPageSection.LinkText),
                new SqlParameter("@link", lendingPageSection.Link),
                new SqlParameter("@status", lendingPageSection.Status),
                new SqlParameter("@listtype", lendingPageSection.ListType),
                new SqlParameter("@topproducts", lendingPageSection.TopProducts),
                new SqlParameter("@categoryid", lendingPageSection.CategoryId),
                new SqlParameter("@isTitleVisible", lendingPageSection.IsTitleVisible),
                new SqlParameter("@titlePosition", lendingPageSection.TitlePosition),
                new SqlParameter("@linkin", lendingPageSection.LinkIn),
                new SqlParameter("@linkPosition", lendingPageSection.LinkPosition),
                new SqlParameter("@backgroundColor", lendingPageSection.BackgroundColor),
                new SqlParameter("@incontainer", lendingPageSection.InContainer),
                new SqlParameter("@titleColor", lendingPageSection.TitleColor),
                new SqlParameter("@textColor", lendingPageSection.TextColor),
                new SqlParameter("@totalRowsInSection", lendingPageSection.TotalRowsInSection),
                new SqlParameter("@isCustomGrid", lendingPageSection.IsCustomGrid),
                new SqlParameter("@numberOfImages", lendingPageSection.NumberOfImages),
                new SqlParameter("@column1", lendingPageSection.Column1),
                new SqlParameter("@column2", lendingPageSection.Column2),
                new SqlParameter("@column3", lendingPageSection.Column3),
                new SqlParameter("@column4", lendingPageSection.Column4),
                new SqlParameter("@modifiedby", lendingPageSection.ModifiedBy),
                new SqlParameter("@modifiedat", lendingPageSection.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.LendingPageSections, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(LendingPageSectionsLibrary lendingPageSection)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", lendingPageSection.Id),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.LendingPageSections, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<LendingPageSectionsLibrary>>> get(LendingPageSectionsLibrary lendingPageSection, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", lendingPageSection.Id),
                new SqlParameter("@lendingPageId", lendingPageSection.LendingPageId),
                new SqlParameter("@layoutId", lendingPageSection.LayoutId),
                new SqlParameter("@layoutTypeId", lendingPageSection.LayoutTypeId),
                new SqlParameter("@name", lendingPageSection.Name),
                new SqlParameter("@lendingPageName", lendingPageSection.LendingPageName),
                new SqlParameter("@layoutTypeName", lendingPageSection.LayoutTypeName),
                new SqlParameter("@layoutName", lendingPageSection.LayoutName),
                new SqlParameter("@status", lendingPageSection.Status),
                new SqlParameter("@searchtext", lendingPageSection.SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetLendingPageSections, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LendingPageSectionsLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<LendingPageSectionsLibrary> lendingPageSection = new List<LendingPageSectionsLibrary>();
            while (await reader.ReadAsync())
            {
                lendingPageSection.Add(new LendingPageSectionsLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    LendingPageId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LendingPageId"))),
                    LayoutId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutId"))),
                    LayoutTypeId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutTypeId"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    Sequence = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Sequence")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Sequence"))),
                    SectionColumns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SectionColumns")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SectionColumns"))),
                    Title = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Title")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Title"))),
                    SubTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle"))),
                    LinkText = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkText")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkText"))),
                    Link = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Link")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Link"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    ListType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ListType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ListType"))),
                    TopProducts = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TopProducts")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TopProducts"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    IsTitleVisible = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsTitleVisible")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsTitleVisible"))),
                    TitlePosition = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitlePosition")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TitlePosition"))),
                    LinkIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkIn")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkIn"))),
                    LinkPosition = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkPosition")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LinkPosition"))),
                    BackgroundColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BackgroundColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BackgroundColor"))),
                    InContainer = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InContainer")))) ? true : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("InContainer"))),
                    TitleColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleColor"))),
                    TextColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TextColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TextColor"))),
                    TotalRowsInSection = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalRowsInSection")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalRowsInSection"))),
                    IsCustomGrid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsCustomGrid")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCustomGrid"))),
                    NumberOfImages = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NumberOfImages")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("NumberOfImages"))),
                    Column1 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column1")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column1"))),
                    Column2 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column2")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column2"))),
                    Column3 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column3")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column3"))),
                    Column4 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column4")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column4"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    LayoutName = Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutName"))),
                    LayoutTypeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeName"))),
                    LendingPageName = Convert.ToString(reader.GetValue(reader.GetOrdinal("LendingPageName"))),
                });
            }
            return lendingPageSection;
        }
    }
}

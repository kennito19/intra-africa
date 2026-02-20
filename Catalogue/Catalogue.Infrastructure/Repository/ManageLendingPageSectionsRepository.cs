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
    public class ManageLendingPageSectionsRepository : IManageLendingPageSectionsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public ManageLendingPageSectionsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(LendingPageSectionsLibrary lendingPageSection)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@lendingPageId", lendingPageSection.LendingPageId),
                    new MySqlParameter("@layoutId", lendingPageSection.LayoutId),
                    new MySqlParameter("@layouttypeId", lendingPageSection.LayoutTypeId),
                    new MySqlParameter("@name", lendingPageSection.Name),
                    new MySqlParameter("@sequence", lendingPageSection.Sequence),
                    new MySqlParameter("@sectionColumns", lendingPageSection.SectionColumns),
                    new MySqlParameter("@title", lendingPageSection.Title),
                    new MySqlParameter("@subTitle", lendingPageSection.SubTitle),
                    new MySqlParameter("@linktext", lendingPageSection.LinkText),
                    new MySqlParameter("@link", lendingPageSection.Link),
                    new MySqlParameter("@status", lendingPageSection.Status),
                    new MySqlParameter("@listtype", lendingPageSection.ListType),
                    new MySqlParameter("@topproducts", lendingPageSection.TopProducts),
                    new MySqlParameter("@categoryid", lendingPageSection.CategoryId),
                    new MySqlParameter("@isTitleVisible", lendingPageSection.IsTitleVisible),
                    new MySqlParameter("@titlePosition", lendingPageSection.TitlePosition),
                    new MySqlParameter("@linkin", lendingPageSection.LinkIn),
                    new MySqlParameter("@linkPosition", lendingPageSection.LinkPosition),
                    new MySqlParameter("@backgroundColor", lendingPageSection.BackgroundColor),
                    new MySqlParameter("@incontainer", lendingPageSection.InContainer),
                    new MySqlParameter("@titleColor", lendingPageSection.TitleColor),
                    new MySqlParameter("@textColor", lendingPageSection.TextColor),
                    new MySqlParameter("@totalRowsInSection", lendingPageSection.TotalRowsInSection),
                    new MySqlParameter("@isCustomGrid", lendingPageSection.IsCustomGrid),
                    new MySqlParameter("@numberOfImages", lendingPageSection.NumberOfImages),
                    new MySqlParameter("@column1", lendingPageSection.Column1),
                    new MySqlParameter("@column2", lendingPageSection.Column2),
                    new MySqlParameter("@column3", lendingPageSection.Column3),
                    new MySqlParameter("@column4", lendingPageSection.Column4),
                    new MySqlParameter("@createdby", lendingPageSection.CreatedBy),
                    new MySqlParameter("@createdat", lendingPageSection.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", lendingPageSection.Id),
                new MySqlParameter("@lendingPageId", lendingPageSection.LendingPageId),
                new MySqlParameter("@layoutId", lendingPageSection.LayoutId),
                new MySqlParameter("@layouttypeId", lendingPageSection.LayoutTypeId),
                new MySqlParameter("@name", lendingPageSection.Name),
                new MySqlParameter("@sequence", lendingPageSection.Sequence),
                new MySqlParameter("@sectionColumns", lendingPageSection.SectionColumns),
                new MySqlParameter("@title", lendingPageSection.Title),
                new MySqlParameter("@subTitle", lendingPageSection.SubTitle),
                new MySqlParameter("@linktext", lendingPageSection.LinkText),
                new MySqlParameter("@link", lendingPageSection.Link),
                new MySqlParameter("@status", lendingPageSection.Status),
                new MySqlParameter("@listtype", lendingPageSection.ListType),
                new MySqlParameter("@topproducts", lendingPageSection.TopProducts),
                new MySqlParameter("@categoryid", lendingPageSection.CategoryId),
                new MySqlParameter("@isTitleVisible", lendingPageSection.IsTitleVisible),
                new MySqlParameter("@titlePosition", lendingPageSection.TitlePosition),
                new MySqlParameter("@linkin", lendingPageSection.LinkIn),
                new MySqlParameter("@linkPosition", lendingPageSection.LinkPosition),
                new MySqlParameter("@backgroundColor", lendingPageSection.BackgroundColor),
                new MySqlParameter("@incontainer", lendingPageSection.InContainer),
                new MySqlParameter("@titleColor", lendingPageSection.TitleColor),
                new MySqlParameter("@textColor", lendingPageSection.TextColor),
                new MySqlParameter("@totalRowsInSection", lendingPageSection.TotalRowsInSection),
                new MySqlParameter("@isCustomGrid", lendingPageSection.IsCustomGrid),
                new MySqlParameter("@numberOfImages", lendingPageSection.NumberOfImages),
                new MySqlParameter("@column1", lendingPageSection.Column1),
                new MySqlParameter("@column2", lendingPageSection.Column2),
                new MySqlParameter("@column3", lendingPageSection.Column3),
                new MySqlParameter("@column4", lendingPageSection.Column4),
                new MySqlParameter("@modifiedby", lendingPageSection.ModifiedBy),
                new MySqlParameter("@modifiedat", lendingPageSection.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", lendingPageSection.Id),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", lendingPageSection.Id),
                new MySqlParameter("@lendingPageId", lendingPageSection.LendingPageId),
                new MySqlParameter("@layoutId", lendingPageSection.LayoutId),
                new MySqlParameter("@layoutTypeId", lendingPageSection.LayoutTypeId),
                new MySqlParameter("@name", lendingPageSection.Name),
                new MySqlParameter("@lendingPageName", lendingPageSection.LendingPageName),
                new MySqlParameter("@layoutTypeName", lendingPageSection.LayoutTypeName),
                new MySqlParameter("@layoutName", lendingPageSection.LayoutName),
                new MySqlParameter("@status", lendingPageSection.Status),
                new MySqlParameter("@searchtext", lendingPageSection.SearchText),
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

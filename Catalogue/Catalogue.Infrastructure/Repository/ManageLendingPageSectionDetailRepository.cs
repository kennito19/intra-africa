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
    public class ManageLendingPageSectionDetailRepository : IManageLendingPageSectionDetailsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public ManageLendingPageSectionDetailRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(LendingPageSectionDetailsLibrary lendingPageSectionDetail)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@lendingPageSectionId", lendingPageSectionDetail.LendingPageSectionId),
                    new MySqlParameter("@layoutTypeDetailsId", lendingPageSectionDetail.LayoutTypeDetailsId),
                    new MySqlParameter("@optionId", lendingPageSectionDetail.OptionId),
                    new MySqlParameter("@image", lendingPageSectionDetail.Image),
                    new MySqlParameter("@ImageAlt", lendingPageSectionDetail.ImageAlt),
                    new MySqlParameter("@isTitleVisible", lendingPageSectionDetail.IsTitleVisible),
                    new MySqlParameter("@title", lendingPageSectionDetail.Title),
                    new MySqlParameter("@subTitle", lendingPageSectionDetail.SubTitle),
                    new MySqlParameter("@titlePosition", lendingPageSectionDetail.TitlePosition),
                    new MySqlParameter("@sequence", lendingPageSectionDetail.Sequence),
                    new MySqlParameter("@columns", lendingPageSectionDetail.Columns),
                    new MySqlParameter("@redirectTo", lendingPageSectionDetail.RedirectTo),
                    new MySqlParameter("@categoryId", lendingPageSectionDetail.CategoryId),
                    new MySqlParameter("@brandIds", lendingPageSectionDetail.BrandIds),
                    new MySqlParameter("@sizeIds", lendingPageSectionDetail.SizeIds),
                    new MySqlParameter("@specificationIds", lendingPageSectionDetail.SpecificationIds),
                    new MySqlParameter("@colorids", lendingPageSectionDetail.ColorIds),
                    new MySqlParameter("@collectionId", lendingPageSectionDetail.CollectionId),
                    new MySqlParameter("@productId", lendingPageSectionDetail.ProductId),
                    new MySqlParameter("@staticPageId", lendingPageSectionDetail.StaticPageId),
                    new MySqlParameter("@customLink", lendingPageSectionDetail.CustomLinks),
                    new MySqlParameter("@titleColor", lendingPageSectionDetail.TitleColor),
                    new MySqlParameter("@subTitleColor", lendingPageSectionDetail.SubTitleColor),
                    new MySqlParameter("@titleSize", lendingPageSectionDetail.TitleSize),
                    new MySqlParameter("@subTitleSize", lendingPageSectionDetail.SubTitleSize),
                    new MySqlParameter("@italicSubTitle", lendingPageSectionDetail.ItalicSubTitle),
                    new MySqlParameter("@italicTitle", lendingPageSectionDetail.ItalicTitle),
                    new MySqlParameter("@description", lendingPageSectionDetail.Description),
                    new MySqlParameter("@sliderType", lendingPageSectionDetail.SliderType),
                    new MySqlParameter("@videoLinkType", lendingPageSectionDetail.VideoLinkType),
                    new MySqlParameter("@videoId", lendingPageSectionDetail.VideoId),
                    new MySqlParameter("@name", lendingPageSectionDetail.Name),
                    new MySqlParameter("@assignCity", lendingPageSectionDetail.AssignCity),
                    new MySqlParameter("@assignState", lendingPageSectionDetail.AssignState),
                    new MySqlParameter("@assignCountry", lendingPageSectionDetail.AssignCountry),
                    new MySqlParameter("@status", lendingPageSectionDetail.Status),
                    new MySqlParameter("@createdby", lendingPageSectionDetail.CreatedBy),
                    new MySqlParameter("@createdat", lendingPageSectionDetail.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.LendingPageSectionDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(LendingPageSectionDetailsLibrary lendingPageSectionDetail)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", lendingPageSectionDetail.Id),
                new MySqlParameter("@lendingPageSectionId", lendingPageSectionDetail.LendingPageSectionId),
                new MySqlParameter("@layoutTypeDetailsId", lendingPageSectionDetail.LayoutTypeDetailsId),
                new MySqlParameter("@optionId", lendingPageSectionDetail.OptionId),
                new MySqlParameter("@image", lendingPageSectionDetail.Image),
                new MySqlParameter("@ImageAlt", lendingPageSectionDetail.ImageAlt),
                new MySqlParameter("@isTitleVisible", lendingPageSectionDetail.IsTitleVisible),
                new MySqlParameter("@title", lendingPageSectionDetail.Title),
                new MySqlParameter("@subTitle", lendingPageSectionDetail.SubTitle),
                new MySqlParameter("@titlePosition", lendingPageSectionDetail.TitlePosition),
                new MySqlParameter("@sequence", lendingPageSectionDetail.Sequence),
                new MySqlParameter("@columns", lendingPageSectionDetail.Columns),
                new MySqlParameter("@redirectTo", lendingPageSectionDetail.RedirectTo),
                new MySqlParameter("@categoryId", lendingPageSectionDetail.CategoryId),
                new MySqlParameter("@brandIds", lendingPageSectionDetail.BrandIds),
                new MySqlParameter("@sizeIds", lendingPageSectionDetail.SizeIds),
                new MySqlParameter("@specificationIds", lendingPageSectionDetail.SpecificationIds),
                new MySqlParameter("@colorids", lendingPageSectionDetail.ColorIds),
                new MySqlParameter("@collectionId", lendingPageSectionDetail.CollectionId),
                new MySqlParameter("@productId", lendingPageSectionDetail.ProductId),
                new MySqlParameter("@staticPageId", lendingPageSectionDetail.StaticPageId),
                new MySqlParameter("@customLink", lendingPageSectionDetail.CustomLinks),
                new MySqlParameter("@titleColor", lendingPageSectionDetail.TitleColor),
                new MySqlParameter("@subTitleColor", lendingPageSectionDetail.SubTitleColor),
                new MySqlParameter("@titleSize", lendingPageSectionDetail.TitleSize),
                new MySqlParameter("@subTitleSize", lendingPageSectionDetail.SubTitleSize),
                new MySqlParameter("@italicSubTitle", lendingPageSectionDetail.ItalicSubTitle),
                new MySqlParameter("@italicTitle", lendingPageSectionDetail.ItalicTitle),
                new MySqlParameter("@description", lendingPageSectionDetail.Description),
                new MySqlParameter("@sliderType", lendingPageSectionDetail.SliderType),
                new MySqlParameter("@videoLinkType", lendingPageSectionDetail.VideoLinkType),
                new MySqlParameter("@videoId", lendingPageSectionDetail.VideoId),
                new MySqlParameter("@name", lendingPageSectionDetail.Name),
                new MySqlParameter("@assignCity", lendingPageSectionDetail.AssignCity),
                new MySqlParameter("@assignState", lendingPageSectionDetail.AssignState),
                new MySqlParameter("@assignCountry", lendingPageSectionDetail.AssignCountry),
                new MySqlParameter("@status", lendingPageSectionDetail.Status),
                new MySqlParameter("@modifiedby", lendingPageSectionDetail.ModifiedBy),
                new MySqlParameter("@modifiedat", lendingPageSectionDetail.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.LendingPageSectionDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(LendingPageSectionDetailsLibrary lendingPageSectionDetail)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", lendingPageSectionDetail.Id),
                new MySqlParameter("@lendingPageSectionId", lendingPageSectionDetail.LendingPageSectionId),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.LendingPageSectionDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<LendingPageSectionDetailsLibrary>>> get(LendingPageSectionDetailsLibrary lendingPageSectionDetail, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", lendingPageSectionDetail.Id),
                new MySqlParameter("@lendingPageSectionId", lendingPageSectionDetail.LendingPageSectionId),
                new MySqlParameter("@layoutTypeDetailsId", lendingPageSectionDetail.LayoutTypeDetailsId),
                new MySqlParameter("@lendingPageSectionname", lendingPageSectionDetail.LendingPageSectionName),
                new MySqlParameter("@optionId", lendingPageSectionDetail.OptionId),
                new MySqlParameter("@status", lendingPageSectionDetail.Status),
                new MySqlParameter("@searchtext", lendingPageSectionDetail.SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetLendingPageSectionDetails, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LendingPageSectionDetailsLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<LendingPageSectionDetailsLibrary> lendingPageSectionDetail = new List<LendingPageSectionDetailsLibrary>();
            while (await reader.ReadAsync())
            {
                lendingPageSectionDetail.Add(new LendingPageSectionDetailsLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    LendingPageSectionId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LendingPageSectionId"))),
                    LayoutTypeDetailsId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeDetailsId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutTypeDetailsId"))),
                    OptionId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OptionId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OptionId"))),
                    Image = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Image")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Image"))),
                    ImageAlt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageAlt")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageAlt"))),
                    IsTitleVisible = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsTitleVisible")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsTitleVisible"))),
                    Title = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Title")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Title"))),
                    SubTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle"))),
                    TitlePosition = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitlePosition")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TitlePosition"))),
                    Sequence = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Sequence"))),
                    Columns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Columns")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Columns"))),
                    RedirectTo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("RedirectTo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RedirectTo"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    BrandIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandIds"))),
                    SizeIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeIds"))),
                    SpecificationIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationIds"))),
                    ColorIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorIds"))),
                    CollectionId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CollectionId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CollectionId"))),
                    ProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    StaticPageId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("StaticPageId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("StaticPageId"))),
                    CustomLinks = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomLinks")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomLinks"))),
                    TitleColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleColor"))),
                    SubTitleColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitleColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitleColor"))),
                    TitleSize = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleSize")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleSize"))),
                    SubTitleSize = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitleSize")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitleSize"))),
                    ItalicSubTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItalicSubTitle")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("ItalicSubTitle"))),
                    ItalicTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItalicTitle")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("ItalicTitle"))),
                    Description = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Description")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    SliderType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SliderType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SliderType"))),
                    VideoLinkType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("VideoLinkType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("VideoLinkType"))),
                    VideoId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("VideoId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("VideoId"))),
                    Name = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Name")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    AssignCity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignCity")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignCity"))),
                    AssignState = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignState")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignState"))),
                    AssignCountry = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignCountry")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignCountry"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    LendingPageSectionName = Convert.ToString(reader.GetValue(reader.GetOrdinal("LendingPageSectionName"))),
                    OptionName = Convert.ToString(reader.GetValue(reader.GetOrdinal("OptionName"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CategoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathName"))),
                });
            }
            return lendingPageSectionDetail;
        }
    }
}

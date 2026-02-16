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
using Catalogue.Domain.DTO;

namespace Catalogue.Infrastructure.Repository
{
    public class ManageHomePageDetailsRepository : IManageHomePageDetailsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public ManageHomePageDetailsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ManageHomePageDetailsLibrary pageDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@sectionId", pageDetails.SectionId),
                    new SqlParameter("@layoutTypeDetailsId", pageDetails.LayoutTypeDetailsId),
                    new SqlParameter("@optionId", pageDetails.OptionId),
                    new SqlParameter("@image", pageDetails.Image),
                    new SqlParameter("@ImageAlt", pageDetails.ImageAlt),
                    new SqlParameter("@isTitleVisible", pageDetails.IsTitleVisible),
                    new SqlParameter("@title", pageDetails.Title),
                    new SqlParameter("@subTitle", pageDetails.SubTitle),
                    new SqlParameter("@titlePosition", pageDetails.TitlePosition),
                    new SqlParameter("@sequence", pageDetails.Sequence),
                    new SqlParameter("@columns", pageDetails.Columns),
                    new SqlParameter("@redirectTo", pageDetails.RedirectTo),
                    new SqlParameter("@categoryId", pageDetails.CategoryId),
                    new SqlParameter("@brandIds", pageDetails.BrandIds),
                    new SqlParameter("@sizeIds", pageDetails.SizeIds),
                    new SqlParameter("@specificationIds", pageDetails.SpecificationIds),
                    new SqlParameter("@colorids", pageDetails.ColorIds),
                    new SqlParameter("@collectionId", pageDetails.CollectionId),
                    new SqlParameter("@productId", pageDetails.ProductId),
                    new SqlParameter("@staticPageId", pageDetails.StaticPageId),
                    new SqlParameter("@lendingPageId", pageDetails.LendingPageId),
                    new SqlParameter("@customLink", pageDetails.CustomLinks),
                    new SqlParameter("@titleColor", pageDetails.TitleColor),
                    new SqlParameter("@subTitleColor", pageDetails.SubTitleColor),
                    new SqlParameter("@titleSize", pageDetails.TitleSize),
                    new SqlParameter("@subTitleSize", pageDetails.SubTitleSize),
                    new SqlParameter("@italicSubTitle", pageDetails.ItalicSubTitle),
                    new SqlParameter("@italicTitle", pageDetails.ItalicTitle),
                    new SqlParameter("@description", pageDetails.Description),
                    new SqlParameter("@sliderType", pageDetails.SliderType),
                    new SqlParameter("@videoLinkType", pageDetails.VideoLinkType),
                    new SqlParameter("@videoId", pageDetails.VideoId),
                    new SqlParameter("@name", pageDetails.Name),
                    new SqlParameter("@assignCity", pageDetails.AssignCity),
                    new SqlParameter("@assignState", pageDetails.AssignState),
                    new SqlParameter("@assignCountry", pageDetails.AssignCountry),
                    new SqlParameter("@status", pageDetails.Status),
                    new SqlParameter("@createdby", pageDetails.CreatedBy),
                new SqlParameter("@createdat", pageDetails.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHomePageDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(ManageHomePageDetailsLibrary pageDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", pageDetails.Id),
                new SqlParameter("@sectionId", pageDetails.SectionId),
                new SqlParameter("@layoutTypeDetailsId", pageDetails.LayoutTypeDetailsId),
                new SqlParameter("@optionId", pageDetails.OptionId),
                new SqlParameter("@image", pageDetails.Image),
                new SqlParameter("@ImageAlt", pageDetails.ImageAlt),
                new SqlParameter("@isTitleVisible", pageDetails.IsTitleVisible),
                new SqlParameter("@title", pageDetails.Title),
                new SqlParameter("@subTitle", pageDetails.SubTitle),
                new SqlParameter("@titlePosition", pageDetails.TitlePosition),
                new SqlParameter("@sequence", pageDetails.Sequence),
                new SqlParameter("@columns", pageDetails.Columns),
                new SqlParameter("@redirectTo", pageDetails.RedirectTo),
                new SqlParameter("@categoryId", pageDetails.CategoryId),
                new SqlParameter("@brandIds", pageDetails.BrandIds),
                new SqlParameter("@sizeIds", pageDetails.SizeIds),
                new SqlParameter("@specificationIds", pageDetails.SpecificationIds),
                new SqlParameter("@colorids", pageDetails.ColorIds),
                new SqlParameter("@collectionId", pageDetails.CollectionId),
                new SqlParameter("@productId", pageDetails.ProductId),
                new SqlParameter("@staticPageId", pageDetails.StaticPageId),
                new SqlParameter("@lendingPageId", pageDetails.LendingPageId),
                new SqlParameter("@customLink", pageDetails.CustomLinks),
                new SqlParameter("@titleColor", pageDetails.TitleColor),
                new SqlParameter("@subTitleColor", pageDetails.SubTitleColor),
                new SqlParameter("@titleSize", pageDetails.TitleSize),
                new SqlParameter("@subTitleSize", pageDetails.SubTitleSize),
                new SqlParameter("@italicSubTitle", pageDetails.ItalicSubTitle),
                new SqlParameter("@italicTitle", pageDetails.ItalicTitle),
                new SqlParameter("@description", pageDetails.Description),
                new SqlParameter("@sliderType", pageDetails.SliderType),
                new SqlParameter("@videoLinkType", pageDetails.VideoLinkType),
                new SqlParameter("@videoId", pageDetails.VideoId),
                new SqlParameter("@name", pageDetails.Name),
                new SqlParameter("@assignCity", pageDetails.AssignCity),
                new SqlParameter("@assignState", pageDetails.AssignState),
                new SqlParameter("@assignCountry", pageDetails.AssignCountry),
                new SqlParameter("@status", pageDetails.Status),
                new SqlParameter("@modifiedby", pageDetails.ModifiedBy),
                new SqlParameter("@modifiedat", pageDetails.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHomePageDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageHomePageDetailsLibrary pageDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", pageDetails.Id),
                new SqlParameter("@sectionId", pageDetails.SectionId),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHomePageDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ManageHomePageDetailsLibrary>>> get(ManageHomePageDetailsLibrary pageDetails, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", pageDetails.Id),
                new SqlParameter("@sectionId", pageDetails.SectionId),
                new SqlParameter("@layoutTypeDetailsId", pageDetails.LayoutTypeDetailsId),
                new SqlParameter("@optionId", pageDetails.OptionId),
                new SqlParameter("@sectionname", pageDetails.SectionName),
                new SqlParameter("@status", pageDetails.Status),
                new SqlParameter("@searchtext", pageDetails.SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageHomePageDetails, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ManageHomePageDetailsLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<ManageHomePageDetailsLibrary> lstLayouts = new List<ManageHomePageDetailsLibrary>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new ManageHomePageDetailsLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    SectionId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SectionId"))),
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
                    LendingPageId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LendingPageId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LendingPageId"))),
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
                    SectionName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SectionName"))),
                    OptionName = Convert.ToString(reader.GetValue(reader.GetOrdinal("OptionName"))),
                });
            }
            return lstLayouts;
        }

        public async Task<BaseResponse<List<FrontHomepageDetailsDto>>> GetFrontHomepageDetails(string Mode, string? Status = null)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@status", Status),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetFrontHomepageDetails, FrontHomepageDetailsAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<FrontHomepageDetailsDto>> FrontHomepageDetailsAsync(DbDataReader reader)
        {
            List<FrontHomepageDetailsDto> lstLayouts = new List<FrontHomepageDetailsDto>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new FrontHomepageDetailsDto()
                {
                    HomePageSectionId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HomePageSectionId"))),
                    LayoutId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutId"))),
                    LayoutTypeId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutTypeId"))),
                    HomePageSectionName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionName"))),
                    HomePageSectionSequence = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionSequence")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HomePageSectionSequence"))),
                    SectionColumns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SectionColumns")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SectionColumns"))),
                    HomePageSectionIsTitleVisible = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionIsTitleVisible")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("HomePageSectionIsTitleVisible"))),
                    HomePageSectionTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionTitle"))),
                    HomePageSectionSubTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionSubTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionSubTitle"))),
                    HomePageSectionTitlePosition = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionTitlePosition")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionTitlePosition"))),
                    HomePageSectionLinkIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionLinkIn")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionLinkIn"))),
                    HomePageSectionLinkText = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionLinkText")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionLinkText"))),
                    HomePageSectionLink = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionLink")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionLink"))),
                    HomePageSectionLinkPosition = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionLinkPosition")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionLinkPosition"))),
                    HomePageSectionStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionStatus")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionStatus"))),
                    ListType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ListType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ListType"))),
                    TopProducts = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TopProducts")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TopProducts"))),
                    TotalRowsInSection = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalRowsInSection")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalRowsInSection"))),
                    IsCustomGrid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsCustomGrid")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCustomGrid"))),
                    NumberOfImages = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NumberOfImages")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("NumberOfImages"))),
                    Column1 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column1")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column1"))),
                    Column2 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column2")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column2"))),
                    Column3 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column3")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column3"))),
                    Column4 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Column4")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Column4"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    BackgroundColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BackgroundColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BackgroundColor"))),
                    InContainer = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InContainer")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("InContainer"))),
                    TitleColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleColor"))),
                    TextColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TextColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TextColor"))),
                    HomePageSectionDetailsId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsId"))),
                    LayoutTypeDetailsId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeDetailsId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutTypeDetailsId"))),
                    OptionId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OptionId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OptionId"))),
                    Image = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Image")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Image"))),
                    ImageAlt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageAlt")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageAlt"))),
                    IsTitleVisible = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsTitleVisible")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsTitleVisible"))),
                    HomePageSectionDetailsTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsTitle"))),
                    HomePageSectionDetailsSubTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsSubTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsSubTitle"))),
                    HomePageSectionDetailsTitlePosition = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsTitlePosition")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsTitlePosition"))),
                    HomePageSectionDetailsSequence = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsSequence")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsSequence"))),
                    Columns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Columns")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Columns"))),
                    RedirectTo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("RedirectTo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RedirectTo"))),
                    HomePageSectionDetailsCategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsCategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsCategoryId"))),
                    BrandIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandIds"))),
                    SizeIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeIds"))),
                    SpecificationIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationIds"))),
                    ColorIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorIds"))),
                    CollectionId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CollectionId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CollectionId"))),
                    ProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    StaticPageId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("StaticPageId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("StaticPageId"))),
                    LendingPageId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LendingPageId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LendingPageId"))),
                    CustomLinks = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomLinks")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomLinks"))),
                    HomePageSectionDetailsTitleColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsTitleColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsTitleColor"))),
                    HomePageSectionDetailsSubTitleColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsSubTitleColor")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsSubTitleColor"))),
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
                    HomePageSectionDetailsStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsStatus")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HomePageSectionDetailsStatus"))),
                    LayoutName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutName"))),
                    LayoutTypeName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeName"))),
                    ClassName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ClassName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ClassName"))),
                    Options = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Options")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Options"))),
                    HasInnerColumns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HasInnerColumns")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("HasInnerColumns"))),
                    LayoutTypeColumns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeColumns")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutTypeColumns"))),
                    MinImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MinImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MinImage"))),
                    MaxImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MaxImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MaxImage"))),
                    LayoutTypeDetailsName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeDetailsName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeDetailsName"))),
                    SectionType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SectionType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SectionType"))),
                    InnerColumns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InnerColumns")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("InnerColumns"))),
                    LayoutOptionName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutOptionName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutOptionName"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CategoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathName"))),
                });
            }
            return lstLayouts;
        }

    }
}

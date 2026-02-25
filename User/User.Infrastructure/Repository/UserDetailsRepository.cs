using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Domain;
using User.Domain.DTO;
using User.Domain.Entity;
using User.Infrastructure.Helper;

namespace User.Infrastructure.Repository
{
    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public UserDetailsRepository(IConfiguration  configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(UserDetails userDetails)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                    new MySqlParameter("@UserId",userDetails.UserId),
                    new MySqlParameter("@UserType",userDetails.UserType),
                    new MySqlParameter("@FirstName",userDetails.FirstName),
                    new MySqlParameter("@LastName",userDetails.LastName),
                    new MySqlParameter("@Status",userDetails.Status),
                    new MySqlParameter("@ProfileImage",userDetails.ProfileImage),
                    new MySqlParameter("@Email",userDetails.Email),
                    new MySqlParameter("@Gender",userDetails.Gender),
                    new MySqlParameter("@Phone",userDetails.Phone),
                    new MySqlParameter("@IsEmailConfirmed",userDetails.IsEmailConfirmed),
                    new MySqlParameter("@IsPhoneConfirmed",userDetails.IsPhoneConfirmed),
                    new MySqlParameter("@CreatedAt",userDetails.CreatedAt),
                    new MySqlParameter("@CreatedBy",userDetails.CreatedBy)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.sp_UserDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                return new BaseResponse<long>
                {
                    code = 400,
                    message = ex.Message,
                    data = 0
                };
            }
        }
        public async Task<BaseResponse<long>> Update(UserDetails userDetails)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","update"),
                    new MySqlParameter("@UserId",userDetails.UserId),
                    new MySqlParameter("@UserType",userDetails.UserType),
                    new MySqlParameter("@FirstName",userDetails.FirstName),
                    new MySqlParameter("@LastName",userDetails.LastName),
                    new MySqlParameter("@Status",userDetails.Status),
                    new MySqlParameter("@ProfileImage",userDetails.ProfileImage),
                    new MySqlParameter("@Email",userDetails.Email),
                    new MySqlParameter("@Gender",userDetails.Gender),
                    new MySqlParameter("@Phone",userDetails.Phone),
                    new MySqlParameter("@IsEmailConfirmed",userDetails.IsEmailConfirmed),
                    new MySqlParameter("@IsPhoneConfirmed",userDetails.IsPhoneConfirmed),
                    new MySqlParameter("@ModifiedAt",userDetails.ModifiedAt),
                    new MySqlParameter("@ModifiedBy",userDetails.ModifiedBy)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.sp_UserDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                return new BaseResponse<long>
                {
                    code = 400,
                    message = ex.Message,
                    data = 0
                };
            }
        }

        public async Task<BaseResponse<long>> Delete(UserDetails userDetails)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","delete"),
                    new MySqlParameter("@UserId",userDetails.UserId),
                    new MySqlParameter("@DeletedBy",userDetails.DeletedBy),
                    new MySqlParameter("@DeletedAt",userDetails.DeletedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.sp_UserDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                return new BaseResponse<long>
                {
                    code = 400,
                    message = ex.Message,
                    data = 0
                };
            }
        }

        public async Task<BaseResponse<List<UserDetailsDTO>>> GetUserDetails(UserDetailsDTO sellerDetails, string? KycStatus, bool? GetArchived, string? SearchText, int? PageIndex, int? PageSize, string? Mode)
        {
            try
            {
                await using var connection = new MySqlConnection(_configuration.GetConnectionString("DBconnection"));
                await connection.OpenAsync();

                await using var countCmd = new MySqlCommand();
                countCmd.Connection = connection;
                var where = new List<string>();

                if (sellerDetails.Id > 0)
                {
                    where.Add("u.Id = @id");
                    countCmd.Parameters.AddWithValue("@id", sellerDetails.Id);
                }
                if (!string.IsNullOrWhiteSpace(sellerDetails.UserId))
                {
                    where.Add("u.UserId = @userId");
                    countCmd.Parameters.AddWithValue("@userId", sellerDetails.UserId);
                }
                if (!string.IsNullOrWhiteSpace(sellerDetails.UserStatus))
                {
                    where.Add("u.UserStatus = @userStatus");
                    countCmd.Parameters.AddWithValue("@userStatus", sellerDetails.UserStatus);
                }
                if (sellerDetails.IsDeleted != null)
                {
                    where.Add("COALESCE(u.IsDeleted,0) = @isDeleted");
                    countCmd.Parameters.AddWithValue("@isDeleted", sellerDetails.IsDeleted.Value);
                }
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    where.Add("(u.UserId LIKE @search OR u.FirstName LIKE @search OR u.LastName LIKE @search OR u.Email LIKE @search OR u.Phone LIKE @search)");
                    countCmd.Parameters.AddWithValue("@search", $"%{SearchText}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;
                countCmd.CommandText = $"SELECT COUNT(1) FROM UserDetails u {whereClause};";
                var totalObj = await countCmd.ExecuteScalarAsync();
                var total = Convert.ToInt32(totalObj ?? 0);

                var result = new List<UserDetailsDTO>();
                if (total > 0)
                {
                    var fetchAll = !PageIndex.HasValue || !PageSize.HasValue || PageIndex <= 0 || PageSize <= 0;
                    var safePageIndex = !PageIndex.HasValue || PageIndex <= 0 ? 1 : PageIndex.Value;
                    var safePageSize = !PageSize.HasValue || PageSize <= 0 ? total : PageSize.Value;
                    var offset = (safePageIndex - 1) * safePageSize;

                    await using var cmd = new MySqlCommand();
                    cmd.Connection = connection;
                    foreach (MySqlParameter p in countCmd.Parameters)
                    {
                        cmd.Parameters.AddWithValue(p.ParameterName, p.Value);
                    }

                    cmd.CommandText = $@"
SELECT
    u.Id,
    u.UserId,
    u.UserType,
    u.FirstName,
    u.LastName,
    u.UserStatus,
    u.ProfileImage,
    u.Email,
    u.Gender,
    u.Phone,
    u.IsEmailConfirmed,
    u.IsPhoneConfirmed,
    u.IsDeleted,
    u.CreatedBy,
    u.CreatedAt,
    u.ModifiedBy,
    u.ModifiedAt,
    u.DeletedBy,
    u.DeletedAt
FROM UserDetails u
{whereClause}
ORDER BY u.Id DESC
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
                        result.Add(new UserDetailsDTO
                        {
                            RowNumber = rowNo,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32("Id"),
                            UserId = reader.IsDBNull("UserId") ? null : reader.GetString("UserId"),
                            UserType = reader.IsDBNull("UserType") ? null : reader.GetString("UserType"),
                            FirstName = reader.IsDBNull("FirstName") ? null : reader.GetString("FirstName"),
                            LastName = reader.IsDBNull("LastName") ? null : reader.GetString("LastName"),
                            UserStatus = reader.IsDBNull("UserStatus") ? null : reader.GetString("UserStatus"),
                            ProfileImage = reader.IsDBNull("ProfileImage") ? null : reader.GetString("ProfileImage"),
                            Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                            Gender = reader.IsDBNull("Gender") ? null : reader.GetString("Gender"),
                            Phone = reader.IsDBNull("Phone") ? null : reader.GetString("Phone"),
                            IsEmailConfirmed = reader.IsDBNull("IsEmailConfirmed") ? null : reader.GetBoolean("IsEmailConfirmed"),
                            IsPhoneConfirmed = reader.IsDBNull("IsPhoneConfirmed") ? null : reader.GetBoolean("IsPhoneConfirmed"),
                            IsDeleted = reader.IsDBNull("IsDeleted") ? null : reader.GetBoolean("IsDeleted"),
                            CreatedBy = reader.IsDBNull("CreatedBy") ? null : reader.GetString("CreatedBy"),
                            CreatedAt = reader.IsDBNull("CreatedAt") ? null : reader.GetDateTime("CreatedAt"),
                            ModifiedBy = reader.IsDBNull("ModifiedBy") ? null : reader.GetString("ModifiedBy"),
                            ModifiedAt = reader.IsDBNull("ModifiedAt") ? null : reader.GetDateTime("ModifiedAt"),
                            DeletedBy = reader.IsDBNull("DeletedBy") ? null : reader.GetString("DeletedBy"),
                            DeletedAt = reader.IsDBNull("DeletedAt") ? null : reader.GetDateTime("DeletedAt"),
                            KYCDetailsId = null,
                            KYCFor = null,
                            DisplayName = null,
                            OwnerName = null,
                            ContactPersonName = null,
                            ContactPersonMobileNo = null,
                            PanCardNo = null,
                            NameOnPanCard = null,
                            DateOfBirth = null,
                            AadharCardNo = null,
                            IsUserWithGST = null,
                            TypeOfCompany = null,
                            CompanyRegistrationNo = null,
                            BussinessType = null,
                            MSMENo = null,
                            AccountNo = null,
                            AccountHolderName = null,
                            BankName = null,
                            AccountType = null,
                            IFSCCode = null,
                            Logo = null,
                            DigitalSign = null,
                            CancelCheque = null,
                            PanCardDoc = null,
                            MSMEDoc = null,
                            AadharCardFrontDoc = null,
                            AadharCardBackDoc = null,
                            ShipmentChargesPaidBy = null,
                            ShipmentBy = null,
                            Note = null,
                            KycStatus = null,
                            GSTInfoDetails = null,
                            WarehouseDetails = null,
                            SellerBrand = null
                        });
                    }
                }

                return new BaseResponse<List<UserDetailsDTO>>
                {
                    code = result.Count > 0 ? 200 : 204,
                    message = result.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserDetailsDTO>>
                {
                    code = 400,
                    message = ex.Message,
                    data = new List<UserDetailsDTO>()
                };
            }
        }

        private async Task<List<UserDetailsDTO>> UserDetailsDataAsync(DbDataReader reader)
        {
            List<UserDetailsDTO> lstSellerDetails = new List<UserDetailsDTO>();
            while (await reader.ReadAsync())
            {
                lstSellerDetails.Add(new UserDetailsDTO()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),

                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    UserId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserId")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    UserType = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserType")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserType"))),
                    FirstName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("FirstName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("FirstName"))),
                    LastName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LastName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LastName"))),
                    UserStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserStatus"))),
                    ProfileImage = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProfileImage")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProfileImage"))),
                    Email = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Email")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Email"))),
                    Gender = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Gender")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Gender"))),
                    Phone = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Phone")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Phone"))),
                    IsEmailConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed"))),
                    IsPhoneConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed"))),
                    IsDeleted = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsDeleted")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    DeletedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = reader.GetValue(reader.GetOrdinal("DeletedAt")) is DBNull ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    CreatedAt = reader.GetValue(reader.GetOrdinal("CreatedAt")) is DBNull ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedAt = reader.GetValue(reader.GetOrdinal("ModifiedAt")) is DBNull ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    CreatedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CreatedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),

                    //KYCDetailsId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("KYCDetailsId"))),

                    KYCDetailsId = reader.IsDBNull(reader.GetOrdinal("KYCDetailsId")) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("KYCDetailsId"))),
                    KYCFor = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("KYCFor")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("KYCFor"))),
                    DisplayName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DisplayName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DisplayName"))),
                    OwnerName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("OwnerName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OwnerName"))),
                    ContactPersonName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ContactPersonName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonName"))),
                    ContactPersonMobileNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ContactPersonMobileNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonMobileNo"))),
                    PanCardNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("PanCardNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PanCardNo"))),
                    NameOnPanCard = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("NameOnPanCard")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("NameOnPanCard"))),
                    DateOfBirth = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DateOfBirth")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DateOfBirth"))),
                    AadharCardNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AadharCardNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AadharCardNo"))),
                    IsUserWithGST = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsUserWithGST")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsUserWithGST"))),
                    TypeOfCompany = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TypeOfCompany")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TypeOfCompany"))),
                    CompanyRegistrationNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CompanyRegistrationNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanyRegistrationNo"))),
                    BussinessType = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("BussinessType")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BussinessType"))),
                    MSMENo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("MSMENo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MSMENo"))),
                    AccountNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AccountNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AccountNo"))),
                    AccountHolderName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AccountHolderName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AccountHolderName"))),
                    BankName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("BankName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BankName"))),
                    AccountType = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AccountType")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AccountType"))),
                    IFSCCode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IFSCCode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("IFSCCode"))),
                    Logo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Logo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Logo"))),
                    DigitalSign = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DigitalSign")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DigitalSign"))),
                    CancelCheque = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CancelCheque")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CancelCheque"))),
                    PanCardDoc = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("PanCardDoc")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PanCardDoc"))),
                    MSMEDoc = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("MSMEDoc")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MSMEDoc"))),
                    AadharCardFrontDoc = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AadharCardFrontDoc")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AadharCardFrontDoc"))),
                    AadharCardBackDoc = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AadharCardBackDoc")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AadharCardBackDoc"))),
                    //ShipmentChargesPaidBy = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ShipmentChargesPaidBy"))),
                    ShipmentChargesPaidBy = reader.IsDBNull(reader.GetOrdinal("ShipmentChargesPaidBy")) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ShipmentChargesPaidBy"))),
                    ShipmentBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ShipmentBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentBy"))),
                    Note = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Note")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Note"))),
                    KycStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("KycStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("KycStatus"))),
                    GSTInfoDetails = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTInfoDetails")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTInfoDetails"))),
                    WarehouseDetails = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("WarehouseDetails")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseDetails"))),
                    SellerBrand = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerBrand")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerBrand"))),
                });
            }
            return lstSellerDetails;
        }

        //public async Task<BaseResponse<List<UserDetailsDTO>>> GetSUserDetails(UserDetailsDTO userDetails, string? UserStatus, bool? GetArchived, int? PageIndex, int? PageSize, string? Mode)
        //{
        //    try
        //    {
        //        var sqlParams = new List<MySqlParameter>() {
        //            new MySqlParameter("@mode", "get"),
        //            new MySqlParameter("@id", userDetails.Id),
        //            new MySqlParameter("@userid", userDetails.UserId),
        //            new MySqlParameter("@userstatus", userDetails.UserStatus),
        //            new MySqlParameter("@getarchived", GetArchived),
        //            new MySqlParameter("@isDeleted", userDetails.IsDeleted),
        //            new MySqlParameter("@pageIndex", PageIndex),
        //            new MySqlParameter("@PageSize", PageSize),
        //        };

        //        MySqlParameter output = new MySqlParameter();
        //        output.ParameterName = "@output";
        //        output.Direction = ParameterDirection.Output;
        //        output.MySqlDbType = MySqlDbType.Int32;

        //        MySqlParameter message = new MySqlParameter();
        //        message.ParameterName = "@message";
        //        message.Direction = ParameterDirection.Output;
        //        message.MySqlDbType = MySqlDbType.VarChar;
        //        message.Size = 50;

        //        return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.sp_GetUserDetails, UserDetailsDataAsync, output, newid: null, message, sqlParams.ToArray());

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //private async Task<List<UserDetailsDTO>> UserDetailsDataAsync(DbDataReader reader)
        //{
        //    List<UserDetailsDTO> lstUserDetails = new List<UserDetailsDTO>();
        //    while (await reader.ReadAsync())
        //    {
        //        lstUserDetails.Add(new UserDetailsDTO()
        //        {
        //            RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
        //            PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
        //            RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),

        //            Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
        //            UserId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserId")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
        //            FirstName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("FirstName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("FirstName"))),
        //            LastName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LastName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LastName"))),
        //            UserStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserStatus"))),
        //            ProfileImage = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProfileImage")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProfileImage"))),
        //            Email = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Email")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Email"))),
        //            Gender = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Gender")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Gender"))),
        //            Phone = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Phone")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Phone"))),
        //            IsEmailConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed"))),
        //            IsPhoneConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed"))),
        //            IsDeleted = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsDeleted")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
        //            DeletedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
        //            DeletedAt = reader.GetValue(reader.GetOrdinal("DeletedAt")) is DBNull ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),

        //            AddressDetails = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AddressDetails")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressDetails"))),
        //            Wishlist = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Wishlist")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Wishlist"))),

        //            //KYCDetailsId = reader.IsDBNull(reader.GetOrdinal("KYCDetailsId")) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("KYCDetailsId"))),
                    
        //        });
        //    }
        //    return lstUserDetails;
        //}
    }
}

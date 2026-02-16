using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public UserDetailsRepository(IConfiguration  configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(UserDetails userDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","add"),
                    new SqlParameter("@UserId",userDetails.UserId),
                    new SqlParameter("@UserType",userDetails.UserType),
                    new SqlParameter("@FirstName",userDetails.FirstName),
                    new SqlParameter("@LastName",userDetails.LastName),
                    new SqlParameter("@Status",userDetails.Status),
                    new SqlParameter("@ProfileImage",userDetails.ProfileImage),
                    new SqlParameter("@Email",userDetails.Email),
                    new SqlParameter("@Gender",userDetails.Gender),
                    new SqlParameter("@Phone",userDetails.Phone),
                    new SqlParameter("@IsEmailConfirmed",userDetails.IsEmailConfirmed),
                    new SqlParameter("@IsPhoneConfirmed",userDetails.IsPhoneConfirmed),
                    new SqlParameter("@CreatedAt",userDetails.CreatedAt),
                    new SqlParameter("@CreatedBy",userDetails.CreatedBy)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.sp_UserDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<long>> Update(UserDetails userDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","update"),
                    new SqlParameter("@UserId",userDetails.UserId),
                    new SqlParameter("@UserType",userDetails.UserType),
                    new SqlParameter("@FirstName",userDetails.FirstName),
                    new SqlParameter("@LastName",userDetails.LastName),
                    new SqlParameter("@Status",userDetails.Status),
                    new SqlParameter("@ProfileImage",userDetails.ProfileImage),
                    new SqlParameter("@Email",userDetails.Email),
                    new SqlParameter("@Gender",userDetails.Gender),
                    new SqlParameter("@Phone",userDetails.Phone),
                    new SqlParameter("@IsEmailConfirmed",userDetails.IsEmailConfirmed),
                    new SqlParameter("@IsPhoneConfirmed",userDetails.IsPhoneConfirmed),
                    new SqlParameter("@ModifiedAt",userDetails.ModifiedAt),
                    new SqlParameter("@ModifiedBy",userDetails.ModifiedBy)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.sp_UserDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Delete(UserDetails userDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","delete"),
                    new SqlParameter("@UserId",userDetails.UserId),
                    new SqlParameter("@DeletedBy",userDetails.DeletedBy),
                    new SqlParameter("@DeletedAt",userDetails.DeletedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.sp_UserDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<UserDetailsDTO>>> GetUserDetails(UserDetailsDTO sellerDetails, string? KycStatus, bool? GetArchived, string? SearchText, int? PageIndex, int? PageSize, string? Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "get"),
                    new SqlParameter("@id", sellerDetails.Id),
                    new SqlParameter("@userid", sellerDetails.UserId),
                    new SqlParameter("@userstatus", sellerDetails.UserStatus),
                    new SqlParameter("@kycstatus", KycStatus),
                    new SqlParameter("@getarchived", GetArchived),
                    new SqlParameter("@isDeleted", sellerDetails.IsDeleted),
                    new SqlParameter("@searchtext", SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.sp_GetUserDetails, UserDetailsDataAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
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
        //        var sqlParams = new List<SqlParameter>() {
        //            new SqlParameter("@mode", "get"),
        //            new SqlParameter("@id", userDetails.Id),
        //            new SqlParameter("@userid", userDetails.UserId),
        //            new SqlParameter("@userstatus", userDetails.UserStatus),
        //            new SqlParameter("@getarchived", GetArchived),
        //            new SqlParameter("@isDeleted", userDetails.IsDeleted),
        //            new SqlParameter("@pageIndex", PageIndex),
        //            new SqlParameter("@PageSize", PageSize),
        //        };

        //        SqlParameter output = new SqlParameter();
        //        output.ParameterName = "@output";
        //        output.Direction = ParameterDirection.Output;
        //        output.SqlDbType = SqlDbType.Int;

        //        SqlParameter message = new SqlParameter();
        //        message.ParameterName = "@message";
        //        message.Direction = ParameterDirection.Output;
        //        message.SqlDbType = SqlDbType.NVarChar;
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

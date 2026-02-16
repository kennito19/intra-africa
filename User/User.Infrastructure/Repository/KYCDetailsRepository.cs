using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Domain;
using User.Domain.DTO;
using User.Domain.Entity;
using User.Infrastructure.Helper;

namespace User.Infrastructure.Repository
{
    public class KYCDetailsRepository : IKYCDetailsRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public KYCDetailsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(KYCDetails kYCDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","add"),
                    new SqlParameter("@userid",kYCDetails.UserID),
                    new SqlParameter("@kycfor",kYCDetails.KYCFor),
                    new SqlParameter("@displayname",kYCDetails.DisplayName),
                    new SqlParameter("@ownername",kYCDetails.OwnerName),
                    new SqlParameter("@contactpersonname",kYCDetails.ContactPersonName),
                    new SqlParameter("@contactpersonmobileno",kYCDetails.ContactPersonMobileNo),
                    new SqlParameter("@pancardno",kYCDetails.PanCardNo),
                    new SqlParameter("@nameonpancard",kYCDetails.NameOnPanCard),
                    new SqlParameter("@dateofbirth",kYCDetails.DateOfBirth),
                    new SqlParameter("@aadharcardno",kYCDetails.AadharCardNo),
                    new SqlParameter("@isuserwithgst",kYCDetails.IsUserWithGST),
                    new SqlParameter("@typeofcompany",kYCDetails.TypeOfCompany),
                    new SqlParameter("@companyregno",kYCDetails.CompanyRegistrationNo),
                    new SqlParameter("@bussinesstype",kYCDetails.BussinessType),
                    new SqlParameter("@msmeno",kYCDetails.MSMENo),
                    new SqlParameter("@accountno",kYCDetails.AccountNo),
                    new SqlParameter("@accountholdername",kYCDetails.AccountHolderName),
                    new SqlParameter("@bankname",kYCDetails.BankName),
                    new SqlParameter("@accounttype",kYCDetails.AccountType),
                    new SqlParameter("@ifsccode",kYCDetails.IFSCCode),
                    new SqlParameter("@logo",kYCDetails.Logo),
                    new SqlParameter("@digitalsign",kYCDetails.DigitalSign),
                    new SqlParameter("@cancelcheque",kYCDetails.CancelCheque),
                    new SqlParameter("@pandoc",kYCDetails.PanCardDoc),
                    new SqlParameter("@msmedoc",kYCDetails.MSMEDoc),
                    new SqlParameter("@aadharcardfrontdoc",kYCDetails.AadharCardFrontDoc),
                    new SqlParameter("@aadharcardbackdoc",kYCDetails.AadharCardBackDoc),
                    new SqlParameter("@shipmentby",kYCDetails.ShipmentBy),
                    new SqlParameter("@shipmentchargespaidby",kYCDetails.ShipmentChargesPaidBy),
                    new SqlParameter("@note",kYCDetails.Note),
                    new SqlParameter("@status",kYCDetails.Status),
                    new SqlParameter("@createdBy",kYCDetails.CreatedBy),
                    new SqlParameter("@createdAt",kYCDetails.CreatedAt)
                    
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.KYCDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<long>> Update(KYCDetails kYCDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@userid", kYCDetails.UserID),
                     new SqlParameter("@displayname",kYCDetails.DisplayName),
                    new SqlParameter("@ownername",kYCDetails.OwnerName),
                    new SqlParameter("@contactpersonname",kYCDetails.ContactPersonName),
                    new SqlParameter("@contactpersonmobileno",kYCDetails.ContactPersonMobileNo),
                    new SqlParameter("@pancardno",kYCDetails.PanCardNo),
                    new SqlParameter("@nameonpancard",kYCDetails.NameOnPanCard),
                    new SqlParameter("@dateofbirth",kYCDetails.DateOfBirth),
                    new SqlParameter("@aadharcardno",kYCDetails.AadharCardNo),
                    new SqlParameter("@isuserwithgst",kYCDetails.IsUserWithGST),
                    new SqlParameter("@typeofcompany",kYCDetails.TypeOfCompany),
                    new SqlParameter("@companyregno",kYCDetails.CompanyRegistrationNo),
                    new SqlParameter("@bussinesstype",kYCDetails.BussinessType),
                    new SqlParameter("@msmeno",kYCDetails.MSMENo),
                    new SqlParameter("@accountno",kYCDetails.AccountNo),
                    new SqlParameter("@accountholdername",kYCDetails.AccountHolderName),
                    new SqlParameter("@bankname",kYCDetails.BankName),
                    new SqlParameter("@accounttype",kYCDetails.AccountType),
                    new SqlParameter("@ifsccode",kYCDetails.IFSCCode),
                    new SqlParameter("@logo",kYCDetails.Logo),
                    new SqlParameter("@digitalsign",kYCDetails.DigitalSign),
                    new SqlParameter("@cancelcheque",kYCDetails.CancelCheque),
                    new SqlParameter("@pandoc",kYCDetails.PanCardDoc),
                    new SqlParameter("@msmedoc",kYCDetails.MSMEDoc),
                    new SqlParameter("@aadharcardfrontdoc",kYCDetails.AadharCardFrontDoc),
                    new SqlParameter("@aadharcardbackdoc",kYCDetails.AadharCardBackDoc),
                    new SqlParameter("@shipmentby",kYCDetails.ShipmentBy),
                    new SqlParameter("@shipmentchargespaidby",kYCDetails.ShipmentChargesPaidBy),
                    new SqlParameter("@note",kYCDetails.Note),
                    new SqlParameter("@status",kYCDetails.Status),
                    new SqlParameter("@modifiedBy", kYCDetails.ModifiedBy),
                    new SqlParameter("@modifiedAt", kYCDetails.ModifiedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.KYCDetails, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Delete(KYCDetails kYCDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", kYCDetails.Id),
                    new SqlParameter("@userid", kYCDetails.UserID),
                    new SqlParameter("@deletedBy", kYCDetails.DeletedBy),
                    new SqlParameter("@deletedAt", kYCDetails.DeletedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.KYCDetails, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<KYCDetails>>> Get(KYCDetails kYCDetails, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", kYCDetails.Id),
                    new SqlParameter("@userid", kYCDetails.UserID),
                    new SqlParameter("@status", kYCDetails.Status),
                    new SqlParameter("@isDeleted", kYCDetails.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetKYCDetails, KYCDetailsParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<KYCDetails>> KYCDetailsParserAsync(DbDataReader reader)
        {
            List<KYCDetails> lstKYCDetails = new List<KYCDetails>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new KYCDetails()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    UserID = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserID"))),
                    KYCFor = Convert.ToString(reader.GetValue(reader.GetOrdinal("KYCFor"))),
                    DisplayName = Convert.ToString(reader.GetValue(reader.GetOrdinal("DisplayName"))),
                    OwnerName = Convert.ToString(reader.GetValue(reader.GetOrdinal("OwnerName"))),
                    ContactPersonName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonName"))),
                    ContactPersonMobileNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonMobileNo"))),
                    PanCardNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("PanCardNo"))),
                    NameOnPanCard = Convert.ToString(reader.GetValue(reader.GetOrdinal("NameOnPanCard"))),
                    DateOfBirth = Convert.ToString(reader.GetValue(reader.GetOrdinal("DateOfBirth"))),
                    AadharCardNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("AadharCardNo"))),
                    IsUserWithGST = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsUserWithGST"))),
                    TypeOfCompany = Convert.ToString(reader.GetValue(reader.GetOrdinal("TypeOfCompany"))),
                    CompanyRegistrationNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanyRegistrationNo"))),
                    BussinessType = Convert.ToString(reader.GetValue(reader.GetOrdinal("BussinessType"))),
                    MSMENo = Convert.ToString(reader.GetValue(reader.GetOrdinal("MSMENo"))),
                    AccountNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("AccountNo"))),
                    AccountHolderName = Convert.ToString(reader.GetValue(reader.GetOrdinal("AccountHolderName"))),
                    BankName = Convert.ToString(reader.GetValue(reader.GetOrdinal("BankName"))),
                    AccountType = Convert.ToString(reader.GetValue(reader.GetOrdinal("AccountType"))),
                    IFSCCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("IFSCCode"))),
                    Logo = Convert.ToString(reader.GetValue(reader.GetOrdinal("Logo"))),
                    DigitalSign = Convert.ToString(reader.GetValue(reader.GetOrdinal("DigitalSign"))),
                    CancelCheque = Convert.ToString(reader.GetValue(reader.GetOrdinal("CancelCheque"))),
                    PanCardDoc = Convert.ToString(reader.GetValue(reader.GetOrdinal("PanCardDoc"))),
                    MSMEDoc = Convert.ToString(reader.GetValue(reader.GetOrdinal("MSMEDoc"))),
                    AadharCardFrontDoc = Convert.ToString(reader.GetValue(reader.GetOrdinal("AadharCardFrontDoc"))),
                    AadharCardBackDoc = Convert.ToString(reader.GetValue(reader.GetOrdinal("AadharCardBackDoc"))),
                    ShipmentBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentBy"))),
                    ShipmentChargesPaidBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ShipmentChargesPaidBy")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ShipmentChargesPaidBy"))),
                    Note = Convert.ToString(reader.GetValue(reader.GetOrdinal("Note"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),

                    FirstName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("FirstName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("FirstName"))),
                    LastName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LastName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LastName"))),
                    UserStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserStatus"))),
                    ProfileImage = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProfileImage")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProfileImage"))),
                    Email = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Email")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Email"))),
                    Gender = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Gender")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Gender"))),
                    Phone = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Phone")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Phone"))),
                    IsEmailConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed"))),
                    IsPhoneConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed"))),
                });
            }
            return lstKYCDetails;
        }

        public async Task<BaseResponse<List<BasicKycDetails>>> GetBasicKycDetails(BasicKycDetails kYCDetails, bool IsDeleted ,int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", kYCDetails.Id),
                    new SqlParameter("@userid", kYCDetails.UserID),
                    new SqlParameter("@status", kYCDetails.Status),
                    new SqlParameter("@kycfor", kYCDetails.KYCFor),
                    new SqlParameter("@isDeleted", IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetBasicKYCDetails, BaicKYCDetailsParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<BasicKycDetails>> BaicKYCDetailsParserAsync(DbDataReader reader)
        {
            List<BasicKycDetails> lstKYCDetails = new List<BasicKycDetails>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new BasicKycDetails()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    UserID = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserID"))),
                    KYCFor = Convert.ToString(reader.GetValue(reader.GetOrdinal("KYCFor"))),
                    DisplayName = Convert.ToString(reader.GetValue(reader.GetOrdinal("DisplayName"))),
                    OwnerName = Convert.ToString(reader.GetValue(reader.GetOrdinal("OwnerName"))),
                    ContactPersonName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonName"))),
                    ContactPersonMobileNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonMobileNo"))),
                    PanCardNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("PanCardNo"))),
                    NameOnPanCard = Convert.ToString(reader.GetValue(reader.GetOrdinal("NameOnPanCard"))),
                    DateOfBirth = Convert.ToString(reader.GetValue(reader.GetOrdinal("DateOfBirth"))),
                    AadharCardNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("AadharCardNo"))),
                    IsUserWithGST = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsUserWithGST"))),
                    TypeOfCompany = Convert.ToString(reader.GetValue(reader.GetOrdinal("TypeOfCompany"))),
                    CompanyRegistrationNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanyRegistrationNo"))),
                    BussinessType = Convert.ToString(reader.GetValue(reader.GetOrdinal("BussinessType"))),
                    MSMENo = Convert.ToString(reader.GetValue(reader.GetOrdinal("MSMENo"))),
                    Logo = Convert.ToString(reader.GetValue(reader.GetOrdinal("Logo"))),
                    DigitalSign = Convert.ToString(reader.GetValue(reader.GetOrdinal("DigitalSign"))),
                    ShipmentBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentBy"))),
                    ShipmentChargesPaidBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ShipmentChargesPaidBy")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ShipmentChargesPaidBy"))),
                    Note = Convert.ToString(reader.GetValue(reader.GetOrdinal("Note"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CreatedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    TradeName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TradeName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TradeName"))),
                    LegalName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LegalName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LegalName"))),
                    GSTNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTNo"))),
                    GSTType = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTType")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTType"))),
                    RegisteredAddressLine1 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine1")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine1"))),
                    RegisteredAddressLine2 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine2")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine2"))),
                    RegisteredLandmark = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredLandmark")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredLandmark"))),
                    RegisteredPincode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredPincode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredPincode"))),
                    City = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("City")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("City"))),
                    State = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("State")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("State"))),
                    Country = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Country")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Country"))),
                    GSTStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTStatus"))),

                    FirstName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("FirstName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("FirstName"))),
                    LastName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LastName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LastName"))),
                    UserStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserStatus"))),
                    ProfileImage = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProfileImage")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProfileImage"))),
                    Email = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Email")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Email"))),
                    Gender = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Gender")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Gender"))),
                    Phone = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Phone")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Phone"))),
                    IsEmailConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed"))),
                    IsPhoneConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed"))),
                });
            }
            return lstKYCDetails;
        }

    }
}

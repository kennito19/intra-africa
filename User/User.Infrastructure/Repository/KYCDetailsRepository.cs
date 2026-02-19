using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
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
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                    new MySqlParameter("@userid",kYCDetails.UserID),
                    new MySqlParameter("@kycfor",kYCDetails.KYCFor),
                    new MySqlParameter("@displayname",kYCDetails.DisplayName),
                    new MySqlParameter("@ownername",kYCDetails.OwnerName),
                    new MySqlParameter("@contactpersonname",kYCDetails.ContactPersonName),
                    new MySqlParameter("@contactpersonmobileno",kYCDetails.ContactPersonMobileNo),
                    new MySqlParameter("@pancardno",kYCDetails.PanCardNo),
                    new MySqlParameter("@nameonpancard",kYCDetails.NameOnPanCard),
                    new MySqlParameter("@dateofbirth",kYCDetails.DateOfBirth),
                    new MySqlParameter("@aadharcardno",kYCDetails.AadharCardNo),
                    new MySqlParameter("@isuserwithgst",kYCDetails.IsUserWithGST),
                    new MySqlParameter("@typeofcompany",kYCDetails.TypeOfCompany),
                    new MySqlParameter("@companyregno",kYCDetails.CompanyRegistrationNo),
                    new MySqlParameter("@bussinesstype",kYCDetails.BussinessType),
                    new MySqlParameter("@msmeno",kYCDetails.MSMENo),
                    new MySqlParameter("@accountno",kYCDetails.AccountNo),
                    new MySqlParameter("@accountholdername",kYCDetails.AccountHolderName),
                    new MySqlParameter("@bankname",kYCDetails.BankName),
                    new MySqlParameter("@accounttype",kYCDetails.AccountType),
                    new MySqlParameter("@ifsccode",kYCDetails.IFSCCode),
                    new MySqlParameter("@logo",kYCDetails.Logo),
                    new MySqlParameter("@digitalsign",kYCDetails.DigitalSign),
                    new MySqlParameter("@cancelcheque",kYCDetails.CancelCheque),
                    new MySqlParameter("@pandoc",kYCDetails.PanCardDoc),
                    new MySqlParameter("@msmedoc",kYCDetails.MSMEDoc),
                    new MySqlParameter("@aadharcardfrontdoc",kYCDetails.AadharCardFrontDoc),
                    new MySqlParameter("@aadharcardbackdoc",kYCDetails.AadharCardBackDoc),
                    new MySqlParameter("@shipmentby",kYCDetails.ShipmentBy),
                    new MySqlParameter("@shipmentchargespaidby",kYCDetails.ShipmentChargesPaidBy),
                    new MySqlParameter("@note",kYCDetails.Note),
                    new MySqlParameter("@status",kYCDetails.Status),
                    new MySqlParameter("@createdBy",kYCDetails.CreatedBy),
                    new MySqlParameter("@createdAt",kYCDetails.CreatedAt)
                    
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@userid", kYCDetails.UserID),
                     new MySqlParameter("@displayname",kYCDetails.DisplayName),
                    new MySqlParameter("@ownername",kYCDetails.OwnerName),
                    new MySqlParameter("@contactpersonname",kYCDetails.ContactPersonName),
                    new MySqlParameter("@contactpersonmobileno",kYCDetails.ContactPersonMobileNo),
                    new MySqlParameter("@pancardno",kYCDetails.PanCardNo),
                    new MySqlParameter("@nameonpancard",kYCDetails.NameOnPanCard),
                    new MySqlParameter("@dateofbirth",kYCDetails.DateOfBirth),
                    new MySqlParameter("@aadharcardno",kYCDetails.AadharCardNo),
                    new MySqlParameter("@isuserwithgst",kYCDetails.IsUserWithGST),
                    new MySqlParameter("@typeofcompany",kYCDetails.TypeOfCompany),
                    new MySqlParameter("@companyregno",kYCDetails.CompanyRegistrationNo),
                    new MySqlParameter("@bussinesstype",kYCDetails.BussinessType),
                    new MySqlParameter("@msmeno",kYCDetails.MSMENo),
                    new MySqlParameter("@accountno",kYCDetails.AccountNo),
                    new MySqlParameter("@accountholdername",kYCDetails.AccountHolderName),
                    new MySqlParameter("@bankname",kYCDetails.BankName),
                    new MySqlParameter("@accounttype",kYCDetails.AccountType),
                    new MySqlParameter("@ifsccode",kYCDetails.IFSCCode),
                    new MySqlParameter("@logo",kYCDetails.Logo),
                    new MySqlParameter("@digitalsign",kYCDetails.DigitalSign),
                    new MySqlParameter("@cancelcheque",kYCDetails.CancelCheque),
                    new MySqlParameter("@pandoc",kYCDetails.PanCardDoc),
                    new MySqlParameter("@msmedoc",kYCDetails.MSMEDoc),
                    new MySqlParameter("@aadharcardfrontdoc",kYCDetails.AadharCardFrontDoc),
                    new MySqlParameter("@aadharcardbackdoc",kYCDetails.AadharCardBackDoc),
                    new MySqlParameter("@shipmentby",kYCDetails.ShipmentBy),
                    new MySqlParameter("@shipmentchargespaidby",kYCDetails.ShipmentChargesPaidBy),
                    new MySqlParameter("@note",kYCDetails.Note),
                    new MySqlParameter("@status",kYCDetails.Status),
                    new MySqlParameter("@modifiedBy", kYCDetails.ModifiedBy),
                    new MySqlParameter("@modifiedAt", kYCDetails.ModifiedAt)
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", kYCDetails.Id),
                    new MySqlParameter("@userid", kYCDetails.UserID),
                    new MySqlParameter("@deletedBy", kYCDetails.DeletedBy),
                    new MySqlParameter("@deletedAt", kYCDetails.DeletedAt)
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", kYCDetails.Id),
                    new MySqlParameter("@userid", kYCDetails.UserID),
                    new MySqlParameter("@status", kYCDetails.Status),
                    new MySqlParameter("@isDeleted", kYCDetails.IsDeleted),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", kYCDetails.Id),
                    new MySqlParameter("@userid", kYCDetails.UserID),
                    new MySqlParameter("@status", kYCDetails.Status),
                    new MySqlParameter("@kycfor", kYCDetails.KYCFor),
                    new MySqlParameter("@isDeleted", IsDeleted),
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

using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Catalogue.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CommissionChargesController : ControllerBase
    {
        private readonly ICommissionChargesService _commissionChargesService;

        public CommissionChargesController(ICommissionChargesService commissionChargesService)
        {
            _commissionChargesService = commissionChargesService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(CommissionChargesLibrary commissionCharges)
        {
            commissionCharges.CreatedAt = DateTime.Now;
            var data = await _commissionChargesService.Create(commissionCharges);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(CommissionChargesLibrary commissionCharges)
        {
            commissionCharges.ModifiedAt = DateTime.Now;
            var data = await _commissionChargesService.Update(commissionCharges);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            CommissionChargesLibrary commissionCharges = new CommissionChargesLibrary();
            commissionCharges.ID = Id;
            commissionCharges.DeletedBy = DeletedBy;
            commissionCharges.DeletedAt = DateTime.Now;
            var data = await _commissionChargesService.Delete(commissionCharges);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<CommissionChargesLibrary>>> get(int ID = 0, int CategoryID = 0, string? SellerID = null, int BrandID = 0, bool? onlyCategory = null, bool? onlySeller = null, bool? onlyBrands = null, bool Isdeleted = false, string? Searchtext = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            CommissionChargesLibrary commissionCharges = new CommissionChargesLibrary();
            commissionCharges.ID = ID;
            if (CategoryID > 0)
            {
                commissionCharges.CatID = CategoryID;
            }
            commissionCharges.SellerID = SellerID;
            if (BrandID > 0)
            {
                commissionCharges.BrandID = BrandID;
            }
            //commissionCharges.ChargesOn = chargeson;
            commissionCharges.IsDeleted = Isdeleted;
            //if (isCompulsary != null)
            //{
            //    commissionCharges.IsCompulsary = isCompulsary;
            //}
            if (onlyCategory != null)
            {
                commissionCharges.OnlyCategories = onlyCategory;
            }
            if (onlySeller != null)
            {
                commissionCharges.OnlySellers = onlySeller;
            }
            if (onlyBrands != null)
            {
                commissionCharges.OnlyBrands = onlyBrands;
            }
            commissionCharges.Searchtext = Searchtext;

            var data = await _commissionChargesService.get(commissionCharges, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("getCategoryWiseCommission")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<CommissionChargesLibrary>>> getCategoryWiseCommission(int CategoryID = 0, string? SellerID = null, int BrandID = 0)
        {
            CommissionChargesLibrary commissionCharges = new CommissionChargesLibrary();
            if (CategoryID > 0)
            {
                commissionCharges.CatID = CategoryID;
            }
            commissionCharges.SellerID = SellerID;
            if (BrandID > 0)
            {
                commissionCharges.BrandID = BrandID;
            }
            //commissionCharges.ChargesOn = chargeson;
            
            var data = await _commissionChargesService.getCategoryWiseCommission(commissionCharges);
            return data;
        }
    }
}

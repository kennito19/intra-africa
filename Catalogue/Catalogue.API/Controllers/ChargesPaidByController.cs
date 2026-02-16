using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Catalogue.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ChargesPaidByController : ControllerBase
    {
        private readonly IChargesPaidByService _chargesPaidByService;

        public ChargesPaidByController(IChargesPaidByService chargesPaidByService)
        {
            _chargesPaidByService = chargesPaidByService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(ChargesPaidByLibrary chargesPaidBy)
        {
            chargesPaidBy.CreatedAt = DateTime.Now;

            var data = await _chargesPaidByService.Create(chargesPaidBy);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(ChargesPaidByLibrary chargesPaidBy)
        {
            chargesPaidBy.ModifiedAt = DateTime.Now;
            var data = await _chargesPaidByService.Update(chargesPaidBy);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ChargesPaidByLibrary chargesPaidBy = new ChargesPaidByLibrary();
            chargesPaidBy.Id = Id;
            chargesPaidBy.DeletedAt = DateTime.Now;
            chargesPaidBy.DeletedBy = DeletedBy;
            var data = await _chargesPaidByService.Delete(chargesPaidBy);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ChargesPaidByLibrary>>> get(int Id = 0, string? Name = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get",string? searchText=null)
        {
            ChargesPaidByLibrary chargesPaidBy = new ChargesPaidByLibrary();
            chargesPaidBy.Id = Id;
            chargesPaidBy.Name = Name;
            chargesPaidBy.IsDeleted = Isdeleted;
            chargesPaidBy.searchText = searchText;

            var data = await _chargesPaidByService.get(chargesPaidBy, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

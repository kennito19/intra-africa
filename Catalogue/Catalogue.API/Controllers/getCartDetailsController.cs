using Catalogue.Application.IServices;
using Catalogue.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Catalogue.Domain.DTO;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class getCartDetailsController : ControllerBase
    {
        private readonly IGetCartListService _getCartListService;

        public getCartDetailsController(IGetCartListService getCartListService)
        {
            _getCartListService = getCartListService;
        }

        [HttpPost("GetCartList")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<GetCheckOutDetailsList>>> GetCartList(getChekoutCalculation CartJson)
        {

            var data = await _getCartListService.GetCartList(CartJson);
            return data;
        }
    }
}

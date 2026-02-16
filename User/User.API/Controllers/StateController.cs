using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Domain.Entity;
using User.Domain;
using Microsoft.AspNetCore.Authorization;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(StateLibrary stateLibrary)
        {
            stateLibrary.CreatedAt = DateTime.Now;
            var data = await _stateService.Create(stateLibrary);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(StateLibrary stateLibrary)
        {
            stateLibrary.CreatedAt = DateTime.Now;
            var data = await _stateService.Update(stateLibrary);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            StateLibrary stateLibrary = new StateLibrary();
            stateLibrary.Id = id;
            stateLibrary.DeletedBy = DeletedBy;
            stateLibrary.DeletedAt = DateTime.Now;
            var data = await _stateService.Delete(stateLibrary);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<StateLibrary>>> Get(int? id = null, string? name = null, int? countryId = null, string? status = null, string? countryIds = null, bool? isDeleted = false, string? countryname = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get",string? searchText=null)
        {
            StateLibrary stateLibrary = new StateLibrary();
            if (id != null)
            {
                stateLibrary.Id = Convert.ToInt32(id);
            }
            if (countryId != null)
            {
                stateLibrary.CountryID = Convert.ToInt32(countryId);
            }
            stateLibrary.Name = name;
            stateLibrary.Status = status;
            stateLibrary.searchText = searchText;
            stateLibrary.IsDeleted = Convert.ToBoolean(isDeleted);
            stateLibrary.CountryName = countryname;
            stateLibrary.countryIds = countryIds;

            var data = await _stateService.Get(stateLibrary, PageIndex, PageSize, Mode);
            return data;
        }
    }
}

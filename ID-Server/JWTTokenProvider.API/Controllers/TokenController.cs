using IDServer.Domain.Entity;
using IDServerApplication.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace JWTTokenProvider.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserRepository _userRespository;
        private readonly ITokenManagerRepository _tokenManagerRepository;


        public TokenController(IUserRepository userRespository, ITokenManagerRepository tokenManagerRepository)
        {
            _userRespository = userRespository;
            _tokenManagerRepository = tokenManagerRepository;
        }

        [HttpGet("GenerateNoAuthToken")]
        public IActionResult Get(string deviceId) 
        {
            string token = _userRespository.GenerateNoAuthToken(deviceId);
            return Ok(token);

        }



        [HttpPost("/GetNewAccessToken")]
        public async Task<IActionResult> GetNewToken(RequestNewTokenModel model)
        {
            if (ModelState.IsValid)
            {
                var newtoken = await _userRespository.generateNewAccessToken(model);
                if(newtoken != null)
                {
                    return Ok(newtoken);
                }
                return Ok(newtoken);
            }
            return BadRequest();
        }
    }
}

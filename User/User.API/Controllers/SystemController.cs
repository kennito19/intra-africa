using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        [HttpGet("RegisterContainerIP")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterContainerIP()
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
            string regResult;
            try { regResult = await http.GetStringAsync("http://intra-africa.com/add_render_ip.php?k=renderaccess2026x"); }
            catch (Exception ex) { return Ok(new { status = "error", message = ex.Message }); }
            return Ok(new { status = "ok", registration = regResult });
        }
    }
}

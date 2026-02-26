using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        [HttpGet("RegisterContainerIP")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterContainerIP()
        {
            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            string ip;
            try { ip = (await http.GetStringAsync("http://api.ipify.org")).Trim(); }
            catch { return Ok(new { status = "error", message = "Could not get outbound IP" }); }

            string regResult;
            try { regResult = await http.GetStringAsync($"http://intra-africa.com/add_render_ip.php?k=renderaccess2026x&ip={ip}"); }
            catch (Exception ex) { regResult = "failed: " + ex.Message; }

            return Ok(new { status = "ok", outbound_ip = ip, registration = regResult });
        }
    }
}

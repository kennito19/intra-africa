using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        [HttpGet("AsiaKolkata")]
        [AllowAnonymous]
        public IActionResult AsiaKolkata()
        {
            TimeZoneInfo timeZone;
            try
            {
                // Windows timezone id
                timeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            }
            catch
            {
                // Linux/macOS timezone id fallback
                timeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            }

            var nowUtc = DateTime.UtcNow;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, timeZone);

            // Keep shape close to worldtimeapi for compatibility with existing frontend logic.
            return Ok(new
            {
                abbreviation = "IST",
                client_ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                datetime = localTime.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                day_of_week = (int)localTime.DayOfWeek,
                day_of_year = localTime.DayOfYear,
                dst = false,
                dst_from = (string?)null,
                dst_offset = 0,
                dst_until = (string?)null,
                raw_offset = 19800,
                timezone = "Asia/Kolkata",
                unixtime = new DateTimeOffset(localTime).ToUnixTimeSeconds(),
                utc_datetime = nowUtc.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                utc_offset = "+05:30",
                week_number = System.Globalization.ISOWeek.GetWeekOfYear(localTime)
            });
        }
    }
}

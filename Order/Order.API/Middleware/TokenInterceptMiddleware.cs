using Microsoft.IdentityModel.Tokens;
using Order.Application.Helper;
using Order.Domain.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace Order.API.Middleware
{
    public class TokenInterceptMiddleware
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly RequestDelegate _next;
        private string _secret;
        private string _validAudience;
        private string _validIssuer;
        private AuthHelper _encLoader;
        public static string IDServerUrl = string.Empty;


        public TokenInterceptMiddleware(RequestDelegate next, IConfiguration config)
        {
            _configuration = config;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            _secret = config["JWT:Secret"];
            _validAudience = config["JWT:ValidAudience"];
            _validIssuer = config["JWT:ValidIssuer"];
            _encLoader = new AuthHelper();
            _next = next;
        }

        //public async Task Invoke(HttpContext httpContext)
        //{
        //    var authHeader = httpContext.Request.Headers["Authorization"];

        //    if (authHeader.Count == 0)
        //    {
        //        await _next(httpContext);
        //        return;
        //    }

        //    string token = authHeader[0].Substring("Bearer ".Length).Trim();

        //    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenParams = new TokenValidationParameters
        //    {
        //        ValidAudience = _validAudience,
        //        ValidIssuer = _validIssuer,
        //        IssuerSigningKey = signingKey,
        //        TokenDecryptionKey = _encLoader.GetDecryptionKey()
        //    };

        //    try
        //    {
        //        var result = await tokenHandler.ValidateTokenAsync(token, tokenParams);

        //        var exp = result.Claims.First(c => c.Key.Equals("exp")).Value;
        //        var expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp.ToString()));

        //        if (expiry.UtcDateTime < DateTime.UtcNow)
        //        {
        //            await httpContext.Response.WriteAsJsonAsync(new KeyValuePair<string, int>("Token Expire", 909));
        //            return;
        //        }
        //    }
        //    catch (SecurityTokenValidationException)
        //    {
        //        await httpContext.Response.WriteAsJsonAsync(new KeyValuePair<string, int>("Token Invalid", 910));
        //        return;
        //    }

        //    await _next(httpContext);
        //}

        public async Task Invoke(HttpContext httpContext)
        {

            var authHeader = httpContext.Request.Headers["Authorization"];
            ApiHelper api = new ApiHelper(httpContext);
            if (authHeader.Count == 0)
            {
                await _next(httpContext);
                return;
            }

            var deviceId = httpContext.Request.Headers["device_id"];
            if (deviceId.Count == 0)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Token Invalid");
                return;
            }

            string token = authHeader[0].Substring("Bearer ".Length).Trim();

            var Apivalue = httpContext.Request.Headers["api_call"];
            bool ApiCall = false;
            if (Apivalue.Count > 0)
            {
                ApiCall = Convert.ToBoolean(Apivalue[0].ToString());
            }
            if (!ApiCall)
            {
                UserSessions userSessions = new UserSessions();
                userSessions.AccessToken = token;
                userSessions.DeviceId = deviceId[0].ToString();

                var response = api.ApiCall(IDServerUrl, "api/Account/userSession", "POST", userSessions);
                if (response.IsSuccessStatusCode)
                {
                    await _next(httpContext);
                    return;

                }
                else if (!response.IsSuccessStatusCode)
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Token Invalid");
                    return;
                }
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenParams = new TokenValidationParameters
            {
                ValidAudience = _validAudience,
                ValidIssuer = _validIssuer,
                IssuerSigningKey = signingKey,
                TokenDecryptionKey = _encLoader.GetDecryptionKey()
            };

            // variable is declared before the try block and assigned DateTimeOffset.MinValue as its initial value ~Sahil
            DateTimeOffset expiry = DateTimeOffset.MinValue;
            try
            {
                var result = await tokenHandler.ValidateTokenAsync(token, tokenParams);
                // here we check: if we not found any claims in a result then we return a token expiration Code/Limit. ~Sahil
                if (result.Claims.Count > 0)
                {
                    var exp = result.Claims.First(c => c.Key.Equals("exp")).Value;
                    expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp.ToString()));
                }

                if (expiry.UtcDateTime < DateTime.UtcNow)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await httpContext.Response.WriteAsJsonAsync(new KeyValuePair<string, int>("Token Expired", 401));
                    return;
                }
            }
            catch (SecurityTokenValidationException)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new KeyValuePair<string, int>("Token Invalid", 401));
                return;
            }

            await _next(httpContext);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenInterceptMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenInterceptMiddleware>();
        }
    }
}

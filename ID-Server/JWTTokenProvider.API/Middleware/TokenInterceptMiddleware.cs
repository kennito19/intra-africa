using IDServer.Domain.Entity;
using IDServer.Infrastructure.Data;
using JWTTokenProvider.API.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace JWTTokenProvider.API.Middleware
{
    public class TokenInterceptMiddleware
    {
        private readonly RequestDelegate _next;
        private string _secret;
        private string _validAudience;
        private string _validIssuer;
        private AuthHelper _encLoader;
        private readonly AspNetIdentityDBContext _dbContext;

        public TokenInterceptMiddleware(RequestDelegate next, IConfiguration config)
        {
            _secret = config["JWT:Secret"];
            _validAudience = config["JWT:ValidAudience"];
            _validIssuer = config["JWT:ValidIssuer"];
            _encLoader = new AuthHelper();
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, AspNetIdentityDBContext dBContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"];

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
                List<UserSessions> lstsessions = dBContext.UserDeviceSessions.Where(x => x.DeviceId == deviceId[0].ToString() && x.AccessToken == token).ToList();
                if (lstsessions.Count() == 0)
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

            try
            {
                var result = await tokenHandler.ValidateTokenAsync(token, tokenParams);

                var exp = result.Claims.First(c => c.Key.Equals("exp")).Value;
                var expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp.ToString()));

                if (expiry.UtcDateTime < DateTime.UtcNow)
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("Token Invalid");
                    return;
                }
            }
            catch (SecurityTokenValidationException)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Token Invalid");
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

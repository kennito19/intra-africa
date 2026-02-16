using IDServer.Domain.Entity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IDServerApplication.IRepositories
{
    public interface ITokenManagerRepository
    {
        string GenerateToken(Users user, List<Claim> claims);

        string GenerateNoAuthToken();

        Task<bool> VerifyTokenAsync(string token);

        string GenerateRefreshToken();

        TokenModel RegenerateToken(Users users, List<Claim> claims, string RefreshToken);

        Task<TokenValidationResult> GetPrincipalFromToken(string? token);
        
    }
}

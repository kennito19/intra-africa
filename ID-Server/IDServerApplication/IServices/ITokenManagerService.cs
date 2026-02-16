using IDServer.Domain.Entity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace IDServerApplication.IServices
{
    public interface ITokenManagerService
    {

        string GenerateToken(Users user, List<Claim> claims);

        Task<bool> VerifyTokenAsync(string token);

        string GenerateRefreshToken();

        TokenModel RegenerateToken(Users users, List<Claim> claims, string RefreshToken);
        Task<TokenValidationResult> GetPrincipalFromToken(string? token);
    }
}

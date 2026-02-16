using IDServer.Domain.Entity;
using IDServerApplication.IRepositories;
using IDServerApplication.IServices;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IDServerApplication.Services
{
    public class TokenManagerService : ITokenManagerService
    {
        private readonly ITokenManagerRepository _tokenManagerRepository;

        public TokenManagerService(ITokenManagerRepository tokenManagerRepository)
        {
            _tokenManagerRepository = tokenManagerRepository;
        }
        public string GenerateToken(Users user, List<Claim> claims)
        {
            var tokenString = _tokenManagerRepository.GenerateToken(user, claims);
            return tokenString;
        }

        public async Task<TokenValidationResult> GetPrincipalFromToken(string? token)
        {
            TokenValidationResult result = await _tokenManagerRepository.GetPrincipalFromToken(token);
            return result;
        }

        public Task<bool> VerifyTokenAsync(string token)
        {
            var payload = _tokenManagerRepository.VerifyTokenAsync(token);
            return payload;
        }
        public string GenerateRefreshToken()
        {
            var data = _tokenManagerRepository.GenerateRefreshToken();
            return data;
        }

        public TokenModel RegenerateToken(Users users, List<Claim> claims, string RefreshToken)
        {
            var data = _tokenManagerRepository.RegenerateToken(users, claims, RefreshToken);
            return data;
        }

    }
}

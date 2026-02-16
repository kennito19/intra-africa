using IDServer.Domain.Entity;
using IDServer.Infrastructure.Auth;
using IDServer.Infrastructure.Certs;
using IDServerApplication.IRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Infrastructure.Repositories
{
    public class TokenManagerRepository : ITokenManagerRepository
    {
        private static IConfiguration _config;
        private List<JwtSecurityToken> listTokens;
        private static RsaSecurityKey encryptionkey;
        private static RSA rsa;
        private HashSet<string> _blacklist = new HashSet<string>();
        private readonly ConcurrentDictionary<string, DateTime> _revokedTokens;

        public TokenManagerRepository(IConfiguration config)
        {
            rsa = RSA.Create(3072);

            var publicKey = File.ReadAllText("./StaticFiles/publicEncryption.pem");
            rsa.ImportFromPem(publicKey);
            encryptionkey = new RsaSecurityKey(rsa);

            _config = config;
            listTokens = new List<JwtSecurityToken>();

            _revokedTokens = new ConcurrentDictionary<string, DateTime>();
        }

        public bool generatePem()
        {
            var PEMProvider = new PEMServiceProvider();

            var encryptionKey = RSA.Create(3072);

            // public key for encryption, private key for decryption
            var publicEncryption = PEMProvider.GeneratePEM(encryptionKey, false);
            var privateEncryption = PEMProvider.GeneratePEM(encryptionKey, true);

            File.WriteAllText("./StaticFiles/publicEncryption.pem", publicEncryption);
            File.WriteAllText("/StaticFiles/privateEncryption.pem", privateEncryption);
            return true;
        }

        public string GenerateToken(Users user, List<Claim> claims)
        {
            bool pem = false;
            if(!File.Exists("./StaticFiles/publicEncryption.pem") || !File.Exists("./StaticFiles/privateEncryption.pem"))
            {
                pem = generatePem();
            }
            else
            {
                pem = true;
            }
            if (pem)
            {

                //var PEMProvider = new PEMServiceProvider();

                //var encryptionKey = RSA.Create(3072);

                // public key for encryption, private key for decryption
                //var publicEncryption = PEMProvider.GeneratePEM(encryptionKey, false);
                //var privateEncryption = PEMProvider.GeneratePEM(encryptionKey, true);

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

                var encryptionKid = _config["JWT:Encryption_Key_id"];


                var privateEncryptionKey = new RsaSecurityKey(rsa) { KeyId = encryptionKid };
                var publicEncryptionKey = new RsaSecurityKey(rsa.ExportParameters(false)) { KeyId = encryptionKid };


                var signingcredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                var encryptioncredentials = new EncryptingCredentials(publicEncryptionKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512);




                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = signingcredentials,
                    EncryptingCredentials = encryptioncredentials,
                    Issuer = _config["JWT:ValidIssuer"],
                    Audience = _config["JWT:ValidAudience"],
                    Subject = new ClaimsIdentity(claims),
                    //NotBefore = DateTime.Now,
                    //Expires = DateTime.Now.AddDays(1)
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(_config["JWT:TokenValidityInDays"]))
                };


                var tokenhandler = new JwtSecurityTokenHandler();

                var token = tokenhandler.CreateJwtSecurityToken(tokenDescriptor);

                //var jwk1 = JsonWebKeyConverter.ConvertFromRSASecurityKey(privateEncryptionKey);
                //var jwk2 = JsonWebKeyConverter.ConvertFromRSASecurityKey(publicEncryptionKey);


                //File.WriteAllText("publicEncryption.pem", publicEncryption);
                //File.WriteAllText("privateEncryption.pem", privateEncryption);

                //File.WriteAllText("publicSignature.pem", publicSigning);
                //File.WriteAllText("privateSignature.pem", privateSigning);


                //var storedJwk = JsonConvert.DeserializeObject<JsonWebKey>(File.ReadAllText("public-rsa.key"));
                //token.SigningKey = JsonConvert.DeserializeObject<JsonWebKey>(File.ReadAllText("privateSignature.pem"));
                //var storeSync = token.SigningKey;



                var tokenString = tokenhandler.WriteToken(token);




                listTokens.Add(token);
                return tokenString;
            }
            else
            {
                return "pem is not correct";
            }
        }
        public async Task<bool> VerifyTokenAsync(string token)
        {
            TokenValidationResult result = await GetPrincipalFromToken(token);
            var payload = result.Claims;
            return result.IsValid;
        }
        public async Task<TokenValidationResult> GetPrincipalFromToken(string? token)
        {
            var encLoader = new EncryptionLoader();
            var tokenhandler = new JwtSecurityTokenHandler();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));


            var tokenparams = new TokenValidationParameters
            {
                ValidAudience = _config["JWT:ValidAudience"],
                ValidIssuer = _config["JWT:ValidIssuer"],

                // public key for signing
                IssuerSigningKey = signingKey,

                // private key for encryption
                TokenDecryptionKey = encLoader.GetDecryptionKey()
            };


            TokenValidationResult result = await tokenhandler.ValidateTokenAsync(token, tokenparams);
            return result;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        public TokenModel RegenerateToken(Users users, List<Claim> claims,string RefreshToken)
        {
            string newAccessToken=GenerateToken(users, claims);
            TokenModel newTokens = new TokenModel(newAccessToken,RefreshToken, 200, "Token generated successfully.");
            return newTokens;
        }

        public string GenerateNoAuthToken()
        {
            List<Claim> claimsList = new List<Claim>();

            claimsList.Add(new Claim("Scope", "general"));

           
            
            
            bool pem = false;
            if (!File.Exists("./StaticFiles/publicEncryption.pem") || !File.Exists("./StaticFiles/privateEncryption.pem"))
            {
                pem = generatePem();
            }
            else
            {
                pem = true;
            }
            if (pem)
            {

                //var PEMProvider = new PEMServiceProvider();

                //var encryptionKey = RSA.Create(3072);

                // public key for encryption, private key for decryption
                //var publicEncryption = PEMProvider.GeneratePEM(encryptionKey, false);
                //var privateEncryption = PEMProvider.GeneratePEM(encryptionKey, true);

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

                var encryptionKid = _config["JWT:Encryption_Key_id"];


                var privateEncryptionKey = new RsaSecurityKey(rsa) { KeyId = encryptionKid };
                var publicEncryptionKey = new RsaSecurityKey(rsa.ExportParameters(false)) { KeyId = encryptionKid };


                var signingcredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                var encryptioncredentials = new EncryptingCredentials(publicEncryptionKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512);




                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    SigningCredentials = signingcredentials,
                    EncryptingCredentials = encryptioncredentials,
                    Issuer = _config["JWT:ValidIssuer"],
                    Audience = _config["JWT:ValidAudience"],
                    Subject = new ClaimsIdentity(claimsList),
                    //NotBefore = DateTime.Now,
                    //Expires = DateTime.Now.AddDays(1)
                    NotBefore = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(_config["JWT:TokenValidityInDays"]))
                };


                var tokenhandler = new JwtSecurityTokenHandler();

                var token = tokenhandler.CreateJwtSecurityToken(tokenDescriptor);

                //var jwk1 = JsonWebKeyConverter.ConvertFromRSASecurityKey(privateEncryptionKey);
                //var jwk2 = JsonWebKeyConverter.ConvertFromRSASecurityKey(publicEncryptionKey);


                //File.WriteAllText("publicEncryption.pem", publicEncryption);
                //File.WriteAllText("privateEncryption.pem", privateEncryption);

                //File.WriteAllText("publicSignature.pem", publicSigning);
                //File.WriteAllText("privateSignature.pem", privateSigning);


                //var storedJwk = JsonConvert.DeserializeObject<JsonWebKey>(File.ReadAllText("public-rsa.key"));
                //token.SigningKey = JsonConvert.DeserializeObject<JsonWebKey>(File.ReadAllText("privateSignature.pem"));
                //var storeSync = token.SigningKey;



                var tokenString = tokenhandler.WriteToken(token);




                listTokens.Add(token);
                return tokenString;
            }
            else
            {
                return "pem is not correct";
            }
        }
    }
}

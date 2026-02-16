using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace JWTTokenProvider.API.Helper
{
    public class AuthHelper
    {
        public RsaSecurityKey GetDecryptionKey()
        {
            var rsa = RSA.Create(3072);

            var privateKey = File.ReadAllText("./StaticFiles/privateEncryption.pem");
            rsa.ImportFromPem(privateKey);
            var encryptedKey = new RsaSecurityKey(rsa);
            return encryptedKey;
        }
    }
}

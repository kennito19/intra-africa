using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Helper
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

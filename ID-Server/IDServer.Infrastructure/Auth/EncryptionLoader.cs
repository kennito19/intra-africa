using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Infrastructure.Auth
{
    public class EncryptionLoader
    {
        public RsaSecurityKey GetDecryptionKey()
        {
            //var PEMProvider = new PEMServiceProvider();

            //var encryptionKey = RSA.Create(3072);

            //// public key for encryption, private key for decryption
            //var publicEncryption = PEMProvider.GeneratePEM(encryptionKey, false);
            //var privateEncryption = PEMProvider.GeneratePEM(encryptionKey, true);

            //File.WriteAllText("./StaticFiles/publicEncryption.pem", publicEncryption);
            //File.WriteAllText("./StaticFiles/privateEncryption.pem", privateEncryption);

            var rsa = RSA.Create(3072);
            var privateKey = File.ReadAllText("./StaticFiles/privateEncryption.pem");
            rsa.ImportFromPem(privateKey);
            var encryptedKey = new RsaSecurityKey(rsa);
            return encryptedKey;
        }
    }
}

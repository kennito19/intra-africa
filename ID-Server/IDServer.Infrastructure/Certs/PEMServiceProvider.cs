using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Infrastructure.Certs
{
    public class PEMServiceProvider
    {
        public PEMServiceProvider()
        {

        }
        public string GeneratePEM(RSA rsa, bool includePrivateParameters = false)
        {
            byte[] keyBytes = includePrivateParameters ? rsa.ExportRSAPrivateKey() : rsa.ExportRSAPublicKey();
            var builder = new StringBuilder($"-----BEGIN RSA {(includePrivateParameters ? "PRIVATE" : "PUBLIC")} KEY");
            builder.AppendLine("-----");

            var base64String = Convert.ToBase64String(keyBytes);
            var offset = 0;
            const int LINE_LENGTH = 64;

            while (offset < base64String.Length)
            {
                var lineEnd = Math.Min(offset + LINE_LENGTH, base64String.Length);
                builder.AppendLine(base64String.Substring(offset, lineEnd - offset));
                offset = lineEnd;
            }

            builder.Append($"-----END RSA {(includePrivateParameters ? "PRIVATE" : "PUBLIC")} KEY");
            builder.AppendLine("-----");
            return builder.ToString();
        }
    }
}

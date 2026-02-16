using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
{
    public class TokenModel
    {
        public int? Code { get; set; }
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public TokenModel()
        {

        }
        public TokenModel(string accesstk, string refreshtk, int code, string message)
        {
            Code = code;
            Message = message;
            AccessToken = accesstk;
            RefreshToken = refreshtk;
        }
    }
}

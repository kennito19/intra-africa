using IDServer.Domain.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
{
    public class SignedInUserResponse
    {
        public int? Code { get; set; }
        public string? Message { get; set; }
        public string? Response { get; set; }

        public UserSignInResponseModel? CurrentUser { get; set; }

        public TokenModel? Tokens { get; set; }
        public List<PageModuleSignInResponse>? PageAccess { get; set; }

        public SignedInUserResponse(SignInResult signIn, UserSignInResponseModel user,TokenModel token, List<PageModuleSignInResponse>? assignPages, string message, int? errorcode = 201)
        {
            Code = !signIn.Succeeded ? errorcode : 200;
            Message = message;
            Response = !signIn.Succeeded ? "Failed" : "Success";
            CurrentUser = user;
            Tokens = token;
            PageAccess = assignPages;
        }

        
    }
}

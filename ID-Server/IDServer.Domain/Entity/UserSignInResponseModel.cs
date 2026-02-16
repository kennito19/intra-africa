using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Domain.Entity
{
    public class UserSignInResponseModel
    {
        public string? UserId { get; set; }
        public string? FullName { get; }
        public string? ProfileImage { get; }
        public string? UserName { get; set; }
        public string? Phone { get; set; }
        public string? UserType { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public bool? IsPhoneConfirmed { get; set; }
        public bool? IsEmailConfirmed { get; set; }


        public UserSignInResponseModel(Users user,string ut,IList<string> roles)
        {
            UserId = user.Id;
            FullName = user.FirstName + " " + user.LastName;
            Status = user.Status;
            Gender = user.Gender;
            ProfileImage = user.ProfileImage??"";
            UserName = user.UserName;
            Phone = user.PhoneNumber;
            UserType = ut;
            Role = roles.First();
            IsPhoneConfirmed = user.PhoneNumberConfirmed;
            IsEmailConfirmed = user.EmailConfirmed;
        }

    }
}

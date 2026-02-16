using IDServer.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Infrastructure.Auth
{
    public class TokenProvider<TUser> : UserManager<TUser> where TUser : class
    {
        private readonly IUserStore<TUser> _store;
        private readonly IServiceProvider _services;

        public TokenProvider(IUserStore<TUser> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger) :
        base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _store = store;
            _services = services;
        }

        public async Task<string> GeneratePasswordResetTokenAsync()
        {
            var random = new Random();
            var randomNum = random.Next(100000, 999999).ToString();

            return randomNum;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(TUser user, string token, string newPassword)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (newPassword == null)
            {
                throw new ArgumentNullException(nameof(newPassword));
            }

            var passwordValidator = _services.GetService<IEnumerable<IPasswordValidator<TUser>>>()?.FirstOrDefault();
            if (passwordValidator != null)
            {
                var result = await passwordValidator.ValidateAsync(this, user, newPassword);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            var removePasswordResult = await RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                return removePasswordResult;
            }

            var addPasswordResult = await AddPasswordAsync(user, newPassword);
            if (!addPasswordResult.Succeeded)
            {
                return addPasswordResult;
            }
            return await UpdateAsync(user);
        }
    }
}

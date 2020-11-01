using AngularShop.Services.Interfaces;
using AngularShop.ViewModels.Account;
using AngularShop.ViewModels.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestAuth.Models;

namespace AngularShop.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ClaimsIdentityService _identityService;
        public AuthService(UserManager<User> userManager, ClaimsIdentityService identityService)
        {
            _userManager = userManager;
            _identityService = identityService;
        }

        public async Task<AuthResponseVM> AuthenticateUser(string Email, string Password)
        {
            var user = await _userManager.FindByNameAsync(Email);

            if (user == null)
                return new AuthResponseVM("User was not found");

            var authenticated = await _userManager.CheckPasswordAsync(user, Password);

            if (!authenticated)
                return new AuthResponseVM("Password incorrect");

            var locked = await _userManager.IsLockedOutAsync(user);

            if (locked)
                return new AuthResponseVM("Account is locked");

            var claims = new Dictionary<string, string>();
            claims.Add(ClaimTypes.NameIdentifier, user.Id.ToString());
            claims.Add(ClaimTypes.Email, Email);
            claims.Add("Role", "Member");
            claims.Add(JwtRegisteredClaimNames.Sub, Email); 
            claims.Add(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
            claims.Add(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString());
            var identity = _identityService.GenerateIdentity(claims);

            return new AuthResponseVM(identity);
        }
    }
}

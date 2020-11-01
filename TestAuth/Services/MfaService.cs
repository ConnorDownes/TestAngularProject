using AngularShop.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using TestAuth.Models;
using System.Web;
using AngularShop.ViewModels.ConfigurationOptions;
using Microsoft.Extensions.Options;
using AngularShop.ViewModels.Shared;
using System.Linq;

namespace AngularShop.Services
{
    public class MfaService : IMfaService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtConfigOptions _jwtConfig;
        public MfaService(UserManager<User> userManager, IOptions<JwtConfigOptions> jwtConfig)
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<SuccessContainer> DisableTwoFactorAsync(User User)
        {
            var mfaEnabled = await _userManager.GetTwoFactorEnabledAsync(User);

            if (!mfaEnabled)
            {
                return new SuccessContainer("2FA is not enabled");
            }

            var outcome = await _userManager.SetTwoFactorEnabledAsync(User, false);

            if (!outcome.Succeeded)
            {
                // Could be modified to return all errors instead of just first
                return new SuccessContainer(outcome.Errors.First().Description);
            }

            return new SuccessContainer();
        }

        public string GenerateQrCodeUri(string email, string key)
        {
            var Issuer = _jwtConfig.Issuer;

            return $"otpauth://totp/{Issuer}:{HttpUtility.UrlEncode(email)}?secret={HttpUtility.UrlEncode(key)}&issuer={Issuer}";
        }

        public async Task<string> GetKeyAsync(User User)
        {
            var key = await _userManager.GetAuthenticatorKeyAsync(User);

            if(String.IsNullOrEmpty(key))
            {
                await _userManager.ResetAuthenticatorKeyAsync(User);
                key = await _userManager.GetAuthenticatorKeyAsync(User);
            }

            return key;
        }

        public async Task<bool> IsMfaEnabledAsync(User User)
        {
            return await _userManager.GetTwoFactorEnabledAsync(User);
        }

        public async Task<SuccessContainerObjectEnumerable<string>> ResetRecoveryCodesAsync(User User, int Count = 10)
        {
            var mfaEnabled = await _userManager.GetTwoFactorEnabledAsync(User);

            if (!mfaEnabled)
            {
                return new SuccessContainerObjectEnumerable<string>("2FA is not enabled");
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(User, Count);

            return new SuccessContainerObjectEnumerable<string>(recoveryCodes);
        }

        public async Task<SuccessContainer> VerifyTokenAsync(User User, string Token)
        {
            string token = Token.Replace(" ", string.Empty).Replace("-", string.Empty);

            var isValid = await _userManager.VerifyTwoFactorTokenAsync(User, _userManager.Options.Tokens.AuthenticatorTokenProvider, token);

            if (!isValid)
            {
                return new SuccessContainer("Verification Token Incorrect");
            }

            return new SuccessContainer();
        }
    }
}

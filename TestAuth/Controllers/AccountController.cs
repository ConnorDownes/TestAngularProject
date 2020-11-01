using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AngularShop.Extensions;
using AngularShop.Models.Data;
using AngularShop.Services.Interfaces;
using AngularShop.ViewModels.Account;
using AngularShop.ViewModels.ConfigurationOptions;
using AngularShop.ViewModels.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TestAuth.Models;

namespace AngularShop.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ShopDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly AppSettingsOptions _appSettings;
        private readonly IAuthService _authService;
        private readonly IMfaService _mfaService;
        public AccountController(UserManager<User> userManager, ShopDbContext context, ITokenService tokenService, IOptions<AppSettingsOptions> appSettings, IAuthService authService, IMfaService mfaService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _appSettings = appSettings.Value;
            _authService = authService;
            _mfaService = mfaService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationVM model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            // TODO: Move this into a users service

            var user = await _userManager.FindByNameAsync(model.Email);

            if(user != null)
            {
                ModelState.AddModelError(model.Password, "An account with this email already exists.");
                return new BadRequestObjectResult(ModelState);
            }

            user.UserName = model.Email;

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                foreach(var error in result.Errors) {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return new BadRequestObjectResult(ModelState);
            }

            await _context.Members.AddAsync(new Member { IdentityId = user.Id });
            await _context.SaveChangesAsync();

            return new OkObjectResult($"Account created: {user}");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var Authentication = await _authService.AuthenticateUser(model.Email, model.Password);

            if(!Authentication.Outcome.IsSuccess)
            {
                return new BadRequestObjectResult(Authentication.Outcome.Error);
            }

            var token = _tokenService.Generate((ClaimsIdentity) Authentication.Identity, _appSettings.TokenDuration);

            return new OkObjectResult(new TokenResponse(token, _appSettings.TokenDuration));
        }

        

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] BaseAuthVM model)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            var Authentication = await _authService.AuthenticateUser(model.Email, model.Password);

            if (!Authentication.Outcome.IsSuccess)
            {
                return new BadRequestObjectResult(Authentication.Outcome.Error);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            var mfaEnabled = await _mfaService.IsMfaEnabledAsync(user);

            return new OkObjectResult(mfaEnabled);
        }

        [Authorize]
        [HttpGet("auth/recovery")]
        public async Task<IActionResult> GetNewRecoveryCodesAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var mfaEnabled = await _mfaService.IsMfaEnabledAsync(user);

            if (!mfaEnabled)
            {
                return new BadRequestObjectResult("2FA is not enabled");
            }

            var recoveryCodes = await _mfaService.ResetRecoveryCodesAsync(user);

            return new OkObjectResult(recoveryCodes.Items);
        }

        [Authorize]
        [HttpGet("auth/verify")]
        public async Task<IActionResult> GetAuthenticatorDetails()
        {
            var user = await _userManager.GetUserAsync(User);
            var mfaEnabled = await _mfaService.IsMfaEnabledAsync(user);

            if (mfaEnabled)
            {
                return new OkObjectResult(new AuthenticatorDetailsVM(mfaEnabled));
            }

            var key = await _mfaService.GetKeyAsync(user);
            var uri = _mfaService.GenerateQrCodeUri(user.Email, key);

            return new OkObjectResult(new AuthenticatorDetailsVM(mfaEnabled, key.Inject(" ", 4), uri));
        }

        [Authorize]
        [HttpGet("auth/status")]
        public async Task<bool> GetAuthenticatorStatus()
        {
            var user = await _userManager.GetUserAsync(User);

            return await _userManager.GetTwoFactorEnabledAsync(user);
        }

        [HttpPost("auth/verify")]
        public async Task<IActionResult> VerifyAuthenticator([FromBody] VerifyAndEmailVM VerifyVM)
        {
            var user = await _userManager.FindByNameAsync(VerifyVM.Email);

            var mfaEnabled = await _mfaService.IsMfaEnabledAsync(user);

            if (!mfaEnabled)
            {
                return new BadRequestObjectResult("2FA is not enabled");
            }

            var outcome = await _mfaService.VerifyTokenAsync(user, VerifyVM.TwoFactorCode);

            if (!outcome.IsSuccess)
            {
                return new BadRequestObjectResult(outcome.Error);
            }

            return new OkObjectResult(true);
        }

        [Authorize]
        [HttpPost("auth/enable")]
        public async Task<IActionResult> EnableAuthenticator([FromBody] VerifyVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            var mfaEnabled = await _mfaService.IsMfaEnabledAsync(user);

            if (mfaEnabled)
            {
                return new BadRequestObjectResult("2FA is already enabled");
            }

            var outcome = await _mfaService.VerifyTokenAsync(user, model.TwoFactorCode);

            if (!outcome.IsSuccess)
            {
                return new BadRequestObjectResult(outcome.Error);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            return new OkObjectResult("2FA Enabled");
        }

        [Authorize]
        [HttpPost("auth/disable")]
        public async Task<IActionResult> DisableAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);

            var outcome = await _mfaService.DisableTwoFactorAsync(user);

            if (!outcome.IsSuccess)
            {
                return new BadRequestObjectResult(outcome.Error);
            }

            return new OkObjectResult("2FA Disabled");
        }

        [HttpGet("exists")]
        public async Task<IActionResult> UserExists(string EmailAddress)
        {
            var user = await _userManager.FindByNameAsync(EmailAddress);

            return new OkObjectResult(user != null);
        }
    }
}

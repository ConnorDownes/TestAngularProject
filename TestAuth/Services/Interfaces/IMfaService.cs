using AngularShop.ViewModels.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAuth.Models;

namespace AngularShop.Services.Interfaces
{
    public interface IMfaService
    {
        Task<bool> IsMfaEnabledAsync(User User);
        Task<string> GetKeyAsync(User User);
        string GenerateQrCodeUri(string email, string key);
        Task<SuccessContainerObjectEnumerable<string>> ResetRecoveryCodesAsync(User User, int Count = 10);
        Task<SuccessContainer> DisableTwoFactorAsync(User User);
        Task<SuccessContainer> VerifyTokenAsync(User User, string Token);
    }
}

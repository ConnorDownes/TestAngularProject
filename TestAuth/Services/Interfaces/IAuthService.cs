using AngularShop.ViewModels.Account;
using AngularShop.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAuth.Models;

namespace AngularShop.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseVM> AuthenticateUser(string Email, string Password);
    }
}

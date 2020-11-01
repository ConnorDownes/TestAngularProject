using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using TestAuth.Models;

namespace AngularShop.Services.Interfaces
{
    public interface ITokenService
    {
        string Generate(IIdentity user, int duration);
    }
}

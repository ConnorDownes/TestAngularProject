using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AngularShop.Services
{
    public class ClaimsIdentityService
    {
        // TODO: INVESTIGATE CLAIMS FACTORY AND WHETHER IT IS WORTH IMPLEMENTING A CUSTOM ONE

        public ClaimsIdentity GenerateIdentity(Dictionary<string, string> Claims)
        {
            List<Claim> ClaimsStore = new List<Claim>();

            foreach (var key in Claims.Keys)
            {
                ClaimsStore.Add(FormatClaim(key, Claims[key]));
            }

            return new ClaimsIdentity(ClaimsStore);
        }

        public ClaimsIdentity GenerateIdentity(IEnumerable<Claim> Claims)
        {
            return new ClaimsIdentity(Claims);
        }

        private Claim FormatClaim(string Name, string value)
        {
            return new Claim(Name, value);
        }
    }
}

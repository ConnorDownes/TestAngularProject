using AngularShop.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.Account
{
    public class AuthResponseVM
    {
        public AuthResponseVM(IIdentity Identity)
        {
            this.Identity = Identity;
            Outcome = new SuccessContainer();
        }

        public AuthResponseVM(string ErrorMessage)
        {
            Outcome = new SuccessContainer(ErrorMessage);
        }

        public IIdentity Identity { get; set; }
        public SuccessContainer Outcome { get; set; }
    }
}

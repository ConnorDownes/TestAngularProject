using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.Account
{
    public class AuthenticatorDetailsVM
    {
        public AuthenticatorDetailsVM(bool IsMfaEnabled)
        {
            this.IsMfaEnabled = IsMfaEnabled;
        }

        public AuthenticatorDetailsVM(bool IsMfaEnabled, string Key, string Uri)
        {
            this.IsMfaEnabled = IsMfaEnabled;
            this.Key = Key;
            this.Uri = Uri;
        }

        public bool IsMfaEnabled { get; set; }
        public string Key { get; set; }
        public string Uri { get; set; }
    }
}

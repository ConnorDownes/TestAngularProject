using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.Security
{
    public class TokenResponse
    {
        public TokenResponse(string Token, int ExpiresIn)
        {
            this.Token = Token;
            this.ExpiresIn = ExpiresIn;
        }

        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}

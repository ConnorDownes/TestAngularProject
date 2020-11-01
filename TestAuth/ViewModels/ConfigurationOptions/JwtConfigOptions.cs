using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.ConfigurationOptions
{
    public class JwtConfigOptions
    {
        public const string JwtConfig = "JwtConfig";

        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.Account
{
    public class VerifyVM
    {
        [Required]
        public string TwoFactorCode { get; set; }
    }
}

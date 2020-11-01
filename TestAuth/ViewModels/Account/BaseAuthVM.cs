using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.Account
{
    public class BaseAuthVM : EmailVM
    {
        [StringLength(20, MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}

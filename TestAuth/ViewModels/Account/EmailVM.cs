using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AngularShop.ViewModels.Account
{
    public class EmailVM
    {
        [StringLength(100, MinimumLength = 3)]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }
}

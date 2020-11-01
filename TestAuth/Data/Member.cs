using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAuth.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string IdentityId { get; set; }
        public User Identity { get; set; }
    }
}

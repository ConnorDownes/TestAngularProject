using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestAuth.Models;

namespace AngularShop.Models.Data
{
    public class ShopDbContext : IdentityDbContext<User>
    {
        public ShopDbContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
    }
}

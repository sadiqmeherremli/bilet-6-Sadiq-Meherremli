using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pigga.Models;

namespace Pigga.DAl
{
    public class AppDbContext : IdentityDbContext<User>

    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Chef> Chefs { get; set; }
    }
}

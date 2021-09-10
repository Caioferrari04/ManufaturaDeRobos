using ManufaturaDeRobos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManufaturaDeRobos.Data
{
    public class ManufactoryContext : IdentityDbContext
    {
        public ManufactoryContext(DbContextOptions<ManufactoryContext> options) : base(options)
        { }

        public DbSet<Robot> Robot { get; set; }
    }
}

using ManufaturaDeRobos.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManufaturaDeRobos.Data
{
    public class ManufactoryContext : IdentityDbContext
    {
        public ManufactoryContext(DbContextOptions<ManufactoryContext> options) : base(options)
        { }

        public DbSet<Robot> Robot { get; set; }
    }
}

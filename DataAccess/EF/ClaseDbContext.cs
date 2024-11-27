using DataAccess.EF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF
{
    public class ClaseDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public ClaseDbContext(DbContextOptions<ClaseDbContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Fine>()
            .Property(f => f.Amount)
            .HasColumnType("decimal(18,2)");

            builder.Entity<Vehicle>()
               .HasOne(v => v.User)
               .WithMany(u => u.Vehicles)
               .HasForeignKey(v => v.UserId);

            builder.Entity<Fine>()
                .HasOne(f => f.User)
                .WithMany(u => u.Fines)
                .HasForeignKey(f => f.UserId);
        }
    }
}

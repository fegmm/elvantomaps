using System;
using System.Collections.Generic;
using System.Text;
using ElvantoMaps.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElvantoMaps.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Location> Locations { get; set; }
        

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Location>()
                .HasIndex(location => new { location.Address, location.Address2, location.City, location.PostCode, location.State, location.Country });
            base.OnModelCreating(builder);
        }
    }
}

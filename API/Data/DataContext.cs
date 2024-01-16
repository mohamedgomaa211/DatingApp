using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add the migration code here
            modelBuilder.Entity<Photo>()
                .Property(p => p.PublicId)
                .HasDefaultValue("default");
            modelBuilder.Entity<AppUser>()
              .Property(u => u.Interests)
              .IsRequired(false); 
            modelBuilder.Entity<AppUser>()
               .Property(u => u.Introduction)
               .IsRequired(false);
            modelBuilder.Entity<AppUser>()
          .Property(u => u.LookingFor)
          .IsRequired(false);


        }

    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shipping_Label_App.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shipping_Label_App.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Labels> Labels { get; set; }
        public DbSet<Classes> classes { get; set; }
        public DbSet<Providers> providers { get; set; }
        public DbSet<ProviderClasses> ProviderClasses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProviderClasses>()
          .HasKey(bc => new { bc.ProviderID, bc.ClassID });

            modelBuilder.Entity<ProviderClasses>()
                .HasOne(bc => bc.Providers)
                .WithMany(b => b.ProviderClasses)
                .HasForeignKey(bc => bc.ProviderID);

            modelBuilder.Entity<ProviderClasses>()
                .HasOne(bc => bc.Classes)
                .WithMany(c => c.ProviderClasses)
                .HasForeignKey(bc => bc.ClassID);
        }
    }    
}

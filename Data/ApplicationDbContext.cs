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
    }
}

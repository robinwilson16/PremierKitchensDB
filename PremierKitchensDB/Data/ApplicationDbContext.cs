using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Address> Address { get; set; }
        public DbSet<AddressType> AddressType { get; set; }
        public DbSet<Area> Area { get; set; }
        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<GetCustomerList> GetCustomerList { get; set; }
        public DbSet<CustomerArea> CustomerArea { get; set; }
        public DbSet<Lookup> Lookup { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<Showroom> Showroom { get; set; }
        public DbSet<SourceOfInformation> SourceOfInformation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Needed to add composite key
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CustomerArea>()
                .HasKey(c => new { c.CustomerID, c.AreaID });

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Lookup>()
                .HasKey(c => new { c.Domain, c.LookupCode });

            //Classes that should not have a table
            //modelBuilder.Ignore<GetCustomerList>();
        }

        public DbSet<PremierKitchensDB.Models.ApplicationRole> ApplicationRole { get; set; }
    }


}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private IConfiguration _config;
        private HttpContext _httpContext;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration config, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _config = config;
            _httpContext = httpContextAccessor.HttpContext;
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

        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbConnection = "";
            dbConnection = "LiveConnection";

            //Comment out below section for PMC Migrations to avoid errors
            var systemDatabase = "";
            if (_httpContext.Request.Cookies["SystemDatabase"] != null)
            {
                systemDatabase = _httpContext.Request.Cookies["SystemDatabase"].ToString();
            }
            else
            {
                systemDatabase = "Live";
            }

            if (systemDatabase == "Training")
            {
                dbConnection = "TrainingConnection";
            }
            else
            {
                dbConnection = "LiveConnection";
            }

            //DO NOT COMMENT OUT THIS PART
            var connString = _config.GetConnectionString(dbConnection);

            optionsBuilder.UseSqlServer(connString);
        }
    }


}

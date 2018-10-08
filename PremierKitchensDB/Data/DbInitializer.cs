using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PremierKitchensDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierKitchensDB.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context,
                          UserManager<ApplicationUser> userManager,
                          RoleManager<ApplicationRole> roleManager,
                          IConfiguration configuration
        )
        {
            context.Database.EnsureCreated();

            // Look for any showrooms.
            if (context.Showroom.Any())
            {
                return;   // DB has been seeded
            }

            var user = await userManager.FindByNameAsync(configuration.GetSection("AppSettings").GetSection("DefaultUsers").GetSection("AdminUser1")["Username"]);
            DateTime createdDate = DateTime.Today;

            var showroom = new Showroom[]
            {
            new Showroom{ShowroomName="-- All --",CreatedDate=createdDate,CreatedBy=user.Id},
            new Showroom{ShowroomName="Bathrooms",CreatedDate=createdDate,CreatedBy=user.Id},
            new Showroom{ShowroomName="Consett",CreatedDate=createdDate,CreatedBy=user.Id},
            new Showroom{ShowroomName="Darlington",CreatedDate=createdDate,CreatedBy=user.Id},
            new Showroom{ShowroomName="Gosforth",CreatedDate=createdDate,CreatedBy=user.Id},
            new Showroom{ShowroomName="Hexham",CreatedDate=createdDate,CreatedBy=user.Id},
            new Showroom{ShowroomName="Morpeth",CreatedDate=createdDate,CreatedBy=user.Id},
            new Showroom{ShowroomName="Whitley Bay",CreatedDate=createdDate,CreatedBy=user.Id},
            };
            foreach (Showroom s in showroom)
            {
                await context.Showroom.AddAsync(s);
            }
            await context.SaveChangesAsync();

            var lookup = new Lookup[]
            {
            new Lookup{Domain="TITLE",LookupCode="Dr",LookupName="Dr",CreatedDate=createdDate,CreatedBy=user.Id},
            new Lookup{Domain="TITLE",LookupCode="Lord &amp; Lady",LookupName="Lord &amp; Lady",CreatedDate=createdDate,CreatedBy=user.Id},
            new Lookup{Domain="TITLE",LookupCode="Miss",LookupName="Miss",CreatedDate=createdDate,CreatedBy=user.Id},
            new Lookup{Domain="TITLE",LookupCode="Mr",LookupName="Mr",CreatedDate=createdDate,CreatedBy=user.Id},
            new Lookup{Domain="TITLE",LookupCode="Mr and Mrs",LookupName="Mr and Mrs",CreatedDate=createdDate,CreatedBy=user.Id},
            new Lookup{Domain="TITLE",LookupCode="Mrs",LookupName="Mrs",CreatedDate=createdDate,CreatedBy=user.Id},
            new Lookup{Domain="TITLE",LookupCode="Ms",LookupName="Ms",CreatedDate=createdDate,CreatedBy=user.Id},
            new Lookup{Domain="TITLE",LookupCode="",LookupName="-- Unknown --",CreatedDate=createdDate,CreatedBy=user.Id}
            };
            foreach (Lookup l in lookup)
            {
                await context.Lookup.AddAsync(l);
            }
            await context.SaveChangesAsync();

            var sourceOfInformation = new SourceOfInformation[]
            {
            new SourceOfInformation{SourceOfInformationName="Chester Le Street Advertiser",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Consett Advertiser",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Durham Advertiser",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Herald and Post",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Hexham Courant",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Internet",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Leaflets",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Morpeth Herald",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="News Post Leader",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Northumberland Gazette",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Passing Factory",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Planning",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Recommend",CreatedDate=createdDate,CreatedBy=user.Id},
            new SourceOfInformation{SourceOfInformationName="Whitley Bay Guardian",CreatedDate=createdDate,CreatedBy=user.Id}
            };
            foreach (SourceOfInformation i in sourceOfInformation)
            {
                await context.SourceOfInformation.AddAsync(i);
            }
            await context.SaveChangesAsync();

            var addressType = new AddressType[]
            {
            new AddressType{AddressTypeName="Home",CreatedDate=createdDate,CreatedBy=user.Id},
            new AddressType{AddressTypeName="Work",CreatedDate=createdDate,CreatedBy=user.Id},
            new AddressType{AddressTypeName="Other",CreatedDate=createdDate,CreatedBy=user.Id}
            };
            foreach (AddressType a in addressType)
            {
                await context.AddressType.AddAsync(a);
            }
            await context.SaveChangesAsync();

            var area = new Area[]
            {
            new Area{AreaName="Bathrooms",CreatedDate=createdDate,CreatedBy=user.Id},
            new Area{AreaName="Bedrooms",CreatedDate=createdDate,CreatedBy=user.Id},
            new Area{AreaName="Kitchens",CreatedDate=createdDate,CreatedBy=user.Id}
            };
            foreach (Area a in area)
            {
                await context.Area.AddAsync(a);
            }
            await context.SaveChangesAsync();
        }
    }
}

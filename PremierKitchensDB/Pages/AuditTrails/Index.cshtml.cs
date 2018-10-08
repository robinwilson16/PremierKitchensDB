﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Pages.AuditTrails
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public IndexModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AuditTrail> AuditTrail { get;set; }

        public async Task OnGetAsync(string tableName, int? id)
        {
            AuditTrail = await _context.AuditTrail
                .Include(a => a.ApplicationUserUpdatedBy)
                .Where(a => a.TableName.ToString() == tableName && a.ObjectID == id)
                .ToListAsync();
        }
    }
}
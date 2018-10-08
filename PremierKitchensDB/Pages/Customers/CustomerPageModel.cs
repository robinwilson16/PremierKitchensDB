using Microsoft.AspNetCore.Mvc.RazorPages;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierKitchensDB.Pages.Customers
{
    public class CustomerPageModel : PageModel
    {
        public List<AssignedAreaData> AssignedAreaDataList;

        public void PopulateCustomerAreaData(ApplicationDbContext context, Customer customer)
        {
            var allAreas = context.Area;
            var customerArea = new HashSet<int>(
                customer.CustomerArea.Select(a => a.AreaID));

            AssignedAreaDataList = new List<AssignedAreaData>();
            foreach (var area in allAreas)
            {
                AssignedAreaDataList.Add(new AssignedAreaData
                {
                    AreaID = area.AreaID,
                    Title = area.AreaName,
                    Assigned = customerArea.Contains(area.AreaID)
                });
            }
        }

        public void UpdateCustomerAreas(ApplicationDbContext context, string[] selectedAreas, Customer customerToUpdate)
        {
            if (selectedAreas == null)
            {
                customerToUpdate.CustomerArea = new List<CustomerArea>();
                return;
            }

            var selectedAreasHS = new HashSet<string>(selectedAreas);
            var customerAreas = new HashSet<int>
                (customerToUpdate.CustomerArea.Select(c => c.Area.AreaID));
            foreach (var area in context.Area)
            {
                if (selectedAreasHS.Contains(area.AreaID.ToString()))
                {
                    if (!customerAreas.Contains(area.AreaID))
                    {
                        customerToUpdate.CustomerArea.Add(
                            new CustomerArea
                            {
                                CustomerID = customerToUpdate.CustomerID,
                                AreaID = area.AreaID
                            });
                    }
                }
                else
                {
                    if (customerAreas.Contains(area.AreaID))
                    {
                        CustomerArea areaToRemove
                            = customerToUpdate
                                .CustomerArea
                                .SingleOrDefault(c => c.AreaID == area.AreaID);
                        context.Remove(areaToRemove);
                    }
                }
            }
        }
    }
}

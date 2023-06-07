using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using assignment4.Data;
using assignment4.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace assignment4.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly assignment4.Data.northwindContext _context;

        public IndexModel(assignment4.Data.northwindContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; } = default!;
        public SelectList SalesReps { get; set; }


        [BindProperty(SupportsGet = true)]
        public int SalesRep { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {

                Order = await _context.Orders
                .Include(o => o.Employee)
                .Include(o => o.ShipViaNavigation).ToListAsync();
            }

            var worker = from w in _context.Orders
                         select w;

            var repQuery = from r in _context.Employees
                           orderby r.EmployeeId
                           select r.EmployeeId;
            SalesReps = new SelectList(await repQuery.Distinct().ToListAsync());

            if (SalesRep > 0)
            {
                worker = worker.Where(w => w.EmployeeId == SalesRep );
            }
        }
    }
}

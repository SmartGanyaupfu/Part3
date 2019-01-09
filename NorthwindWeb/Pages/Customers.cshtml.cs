using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Packt.CS7;

namespace NorthwindWeb.Pages
{
    public class CustomersModel : PageModel
    {
        private Northwind db;
        [BindProperty]
        public Customer Customer { get; set; }
        public IEnumerable<string> Customers { get; set; }
        public CustomersModel(Northwind injectedContext)
        {
            db = injectedContext;
        }
        public void OnGet()
        {
            ViewData["Title"] = "Northwind Web Site- Suppliers";
            // Suppliers = new[] { "Alpha Co", "Smart Limited", "Gammatox" };
            Customers = db.Customers.Select(s => s.CompanyName).ToArray();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Packt.CS7;

namespace NorthwindWeb.Pages
{
    public class SuppliersModel : PageModel
    {
        private Northwind db;
        [BindProperty]
        public Supplier Supplier { get; set; }
        public IEnumerable<string> Suppliers { get; set; }

        public SuppliersModel(Northwind injectedContext)
        {
            db = injectedContext;
        }
        public void OnGet()
        {
            ViewData["Title"] = "Northwind Web Site- Suppliers";
            // Suppliers = new[] { "Alpha Co", "Smart Limited", "Gammatox" };
            Suppliers = db.Suppliers.Select(s => s.CompanyName).ToArray();
        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(Supplier);
                db.SaveChanges();
                return RedirectToPage("/suppliers");
            }

            return Page();
        }
    }
}
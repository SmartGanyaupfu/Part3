using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Packt.CS7;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NorthwindService.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly Northwind db;
         public ProductsController (Northwind db)
        {
            this.db = db;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var products = db.Products.ToArray();
            return products;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IEnumerable<Product> GetByCategory (int id)
        {
            var products = db.Products.Where(p => p.CategoryID == id).ToArray(); ;
            return products;
        }

        
    }
}

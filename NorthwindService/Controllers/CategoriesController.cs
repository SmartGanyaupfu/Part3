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
    public class CategoriesController : Controller
    {
        private readonly Northwind db;

        public CategoriesController (Northwind db)
        {
            this.db = db;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            var categories = db.Categories.ToArray();
            return categories;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            var category = db.Categories.Find(id);
            return category;
        }

       

   
    }
}

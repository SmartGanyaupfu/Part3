using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Packt.CS7;
using NorthwindService.Repositories;

namespace NorthwindService.Controllers
{
    //base address :api/customers
    [Route("api/[controller]")]
    public class CustomersController:Controller
    {
        private ICustomerRepository repo;
        // constructor injects registered repository
        public CustomersController(ICustomerRepository repo)
        {
            this.repo = repo;
        }

        //GET api/customers
        //GET: api/customers/?=[country]

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomers(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                return await repo.RetrieveAllAsync();
            }
            else
            {
                return (await repo.RetrieveAllAsync()).Where(customer => customer.Country == country);

            }
        }
        //Get: api/customers/[id]
        [HttpGet("{id}",Name = "GetCustomer")]
        public async Task<IActionResult> GetCustomer(string id)
        {
            Customer c = await repo.RetriveAsync(id);
            if (c == null)
            {
                return NotFound();//404 resource not found
            }
            return new ObjectResult(c);//200 ok
        }

        //Post : api/customers
        //Body:Customer(Json/xml)
        [HttpPost]
        public async Task<IActionResult> Create ([FromBody] Customer c)
        {
            if (c==null)
            {
                return BadRequest(); //400 Bad Request
            }
            Customer added = await repo.CreateAsync(c);
            return CreatedAtRoute("GetCustomer",//use named route
                new { id = added.CustomerID.ToLower() }, c);//201 created
        }

        //Put: api/customers/[id]
        //Body:Customer (json,xml)
        [HttpPut("{id}")]
        public async Task <IActionResult> Update (string id, [FromBody] Customer c)
        {
            id = id.ToUpper();
            c.CustomerID = c.CustomerID.ToUpper();
            if (c==null || c.CustomerID != id)
            {
                return BadRequest();//
            }
            var existing = await repo.RetriveAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            await repo.UpdateAsync(id, c);
            return new NoContentResult();// 204 no content
        }

        //Delete api/customers/[id]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (string id)
        {
            var existing = await repo.RetriveAsync(id);
            if (existing== null)
            {
                return NotFound();
            }
            bool deleted = await repo.DeleteAsync(id);
            if (deleted)
            {
                return new NoContentResult();
            }
            else
            {
                return BadRequest();
            }
        }


    }
}

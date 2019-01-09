﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Packt.CS7;

namespace NorthwindService.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer c);
        Task<IEnumerable<Customer>> RetrieveAllAsync();
        Task<Customer> RetriveAsync(string id);
        Task<Customer> UpdateAsync(string id, Customer c);
        Task<bool> DeleteAsync(string id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Packt.CS7;
using System.Collections.Concurrent;

namespace NorthwindService.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        // cach the customers in a thread-safe dictionary to improve performance
        private static ConcurrentDictionary<string, Customer> customersCache;

        private Northwind db;

        public CustomerRepository(Northwind db)
        {
            this.db = db;
            //preload the customers from the db as normal
            //Dictionary with CustomerId is the key
            //then convert to a thread safe concurrentDictionary
            if (customersCache == null)
            {
                customersCache = new ConcurrentDictionary<string, Customer>(db.Customers.ToDictionary(c => c.CustomerID));

            }
        }

        public async Task<Customer> CreateAsync(Customer c)
        {
            //normalize customerID into uppercase
            c.CustomerID = c.CustomerID.ToUpper();

            //add to database ussing EF Core
            EntityEntry<Customer> added = await db.Customers.AddAsync(c);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                //if the customer is new, add it cache else
                // call UpdateCache method

                return customersCache.AddOrUpdate(c.CustomerID, c, UpdateCache);
            }
            else
            {
                return null; 
            }

        }

          
    
        public async Task<IEnumerable<Customer>> RetrieveAllAsync()
        {
            // for performance get from cache
            return await Task.Run<IEnumerable<Customer>>(() => customersCache.Values); 
        }

        public async Task<Customer> RetriveAsync(string id)
        {
            return await Task.Run(() =>
            {
                //for performance get from cache
                id = id.ToUpper();
                Customer c;
                customersCache.TryGetValue(id, out c);
                return c;

            });
               
        }

        private Customer UpdateCache (string id,Customer c)
        {
            Customer old;
            if (customersCache.TryGetValue(id,out old))
            {
                if (customersCache.TryUpdate(id, c, old))
                {
                    return c;
                }
            }
            return null;
        }
        public async Task<Customer> UpdateAsync(string id, Customer c)
        {
            return await Task.Run(() =>
            {
                //normalize customer id
                id = id.ToUpper();
                c.CustomerID = c.CustomerID.ToUpper();

                //update in database
                db.Customers.Update(c);
                int affected = db.SaveChanges();
                if (affected==1)
                {
                    //update in cache
                    return Task.Run(() => UpdateCache(id, c));
                }
                return null;
            });
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await Task.Run(() =>
            {
                id = id.ToUpper();
                //refrom from database
                Customer c = db.Customers.Find(id);
                db.Customers.Remove(c);
                int affected = db.SaveChanges();
                if (affected == 1)
                {
                    //remove from cache
                    return Task.Run(() => customersCache.TryRemove(id, out c));
                }
                else
                {
                    return null;
                }
            });
        }// delte
    }
}

using DataAccessObject.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccessObject.Implements
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BanHangDbContext _context;

        public CustomerRepository(BanHangDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> CheckLogin(string Email, string Password)
        {
            return await _context.Customer.Where(x => x.CustomerEmail == Email && x.CustomerPassword == Password).FirstOrDefaultAsync();
        }

        public async Task<Customer> Create(Customer data)
        {
            var result = await _context.Customer.AddAsync(data);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Customer> GetCustomerById(int Id)
        {
            return await _context.Customer.FirstOrDefaultAsync(x => x.CustomerId == Id);
        }

        public async Task<IEnumerable<Customer>> GetCustomers(int page)
        {
            return await _context.Customer.ToListAsync();
        }

        public async Task<bool> UpdateInformation(Customer data)
        {
            var customer = await _context.Customer.FirstOrDefaultAsync(x => x.CustomerId == data.CustomerId);

            if (customer == null)
                return false;
            
            customer.CustomerFullname = data.CustomerFullname;
            customer.CustomerAddress = data.CustomerAddress;
            customer.CustomerPhone = data.CustomerPhone;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task UpdatePassword(int CustomerId, string NewPassword)
        {
            var customer = await _context.Customer.Where(x => x.CustomerId == CustomerId).FirstOrDefaultAsync();
            customer.CustomerPassword = NewPassword;
            await _context.SaveChangesAsync();
        }
    }
}

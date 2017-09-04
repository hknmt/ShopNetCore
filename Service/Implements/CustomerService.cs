using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessObject;
using System.Threading.Tasks;
using DataAccessObject.Interfaces;

namespace Service.Implements
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerReposistory;

        public CustomerService(ICustomerRepository customerReposistory)
        {
            _customerReposistory = customerReposistory;
        }

        public async Task<Customer> CheckLogin(string Email, string Password)
        {
            return await _customerReposistory.CheckLogin(Email, Password);
        }

        public async Task<Customer> Create(Customer data)
        {
            return await _customerReposistory.Create(data);
        }

        public async Task<Customer> GetCustomerById(int Id)
        {
            return await _customerReposistory.GetCustomerById(Id);
        }

        public async Task<IEnumerable<Customer>> GetCustomers(int page)
        {
            return await _customerReposistory.GetCustomers(page);
        }

        public async Task<bool> UpdateInformation(Customer data)
        {
            return await _customerReposistory.UpdateInformation(data);
        }
    }
}

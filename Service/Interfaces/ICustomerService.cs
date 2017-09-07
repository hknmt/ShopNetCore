﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> Create(Customer data);
        Task<bool> UpdateInformation(Customer data);
        Task<Customer> GetCustomerById(int Id);
        Task<IEnumerable<Customer>> GetCustomers(int page);
        Task<Customer> CheckLogin(string Email, string Password);
        Task UpdatePassword(int CustomerId, string NewPassword);
    }
}

using Account.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Services
{
    public interface IAccountRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task<int> CreateAccountAsync(Customer customer);
        Task<Customer> GetCustomerAsync(string email);
        Task<Models.Account> GetAccountAsync(Guid accountId);
        Task<Guid> GetAccountIdByCustomerIdAsync(Guid customerId);
    }
}

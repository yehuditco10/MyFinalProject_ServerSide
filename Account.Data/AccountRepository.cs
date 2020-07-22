using Account.Data.Entities;
using Account.Data.Exceptions;
using Account.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Account.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;

        public AccountRepository(AccountContext accountContext,
            IMapper mapper)
        {
            _accountContext = accountContext;
            _mapper = mapper;
        }
        public async Task<bool> CreateAccountAsync(Services.Models.Customer customerModel)
        {
            try
            {
                Customer newCustomer = _mapper.Map<Customer>(customerModel);
                newCustomer.Id = Guid.NewGuid();
                newCustomer.Active = false;
                _accountContext.Customers.Add(newCustomer);
                var account = new Entities.Account()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = newCustomer.Id,
                    Opendate = DateTime.Today,
                    Balance = 100000,
                };
                _accountContext.Accounts.Add(account);
                 await _accountContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new CreateAccountFailed($"Account creation for { customerModel.Email } failed");
            }
        }
        public async Task<bool> IsEmailExistsAsync(string email)
        {
            Entities.Customer customer = await _accountContext.Customers.FirstOrDefaultAsync(
                c => c.Email == email);
            if (customer == null)
            {
                return false;
            }
            return true;
        }
        public async Task<Services.Models.Customer> GetCustomerAsync(string email)
        {
            try
            {

                Entities.Customer customer = await _accountContext.Customers
                  .FirstOrDefaultAsync(c => c.Email == email);
                if (customer == null)
                    throw new AccountNotFoundException("Your email is not exist");
                return _mapper.Map<Services.Models.Customer>(customer);
            }
            catch (Exception e)
            {
                throw new AccountNotFoundException(e.Message);
            }
        }
        public async Task<Services.Models.Account> GetAccountAsync(Guid accountId)
        {
            try
            {
                var account = await _accountContext.Accounts
                   .Include(c => c.Customer)
                     .FirstOrDefaultAsync(a => a.Id == accountId);
                if (account != null)
                {
                    return _mapper.Map<Services.Models.Account>(account);
                }
                throw new AccountNotFoundException($"There is no account with id {accountId}");
            }
            catch (Exception e)
            {
                throw new AccountNotFoundException(e.Message);
            }
        }
        public async Task<Guid> GetAccountIdByCustomerIdAsync(Guid customerId)
        {
            try
            {
                var account = await _accountContext.Accounts
                   .FirstOrDefaultAsync(a => a.CustomerId == customerId);
                if (account != null)
                {
                    return account.Id;
                }
                throw new AccountNotFoundException($"There is no account for customer {customerId}");
            }
            catch (Exception e)
            {
                throw new AccountNotFoundException(e.Message);
            }
        }
    }
}

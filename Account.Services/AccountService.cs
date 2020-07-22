using Account.Services.Exceptions;
using Account.Services.Models;
using System;
using System.Threading.Tasks;

namespace Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<bool> CreateAsync(Customer customer)
        {
            var isExsits = _accountRepository.IsEmailExistsAsync(customer.Email);
            if (isExsits.Result == false)
            {
                customer.PasswordSalt = Hashing.GetSalt();
                customer.PasswordHash = Hashing.GenerateHash(customer.Password,customer.PasswordSalt);
                return await _accountRepository.CreateAccountAsync(customer);
            }
            return false;
        }
        public async Task<Models.Account> GetAccountAsync(Guid accountId)
        {
            return await _accountRepository.GetAccountAsync(accountId);
        }
        public async Task<Guid> LoginAsync(string email, string password)
        {
            Customer customer = await _accountRepository.GetCustomerAsync(email);
            bool isValid = VerifyHashedPassword(customer.PasswordHash,customer.PasswordSalt, password);
            if (isValid == false)
              throw new LoginFailedException("Your password is not valid");
            return await _accountRepository.GetAccountIdByCustomerIdAsync(customer.Id);
        }

        private bool VerifyHashedPassword( string customerPassword,string customerSaltPassword,string passwordFromUser)
        {
            if (Hashing.AreEqual(passwordFromUser, customerPassword, customerSaltPassword))
                return true;
            return false;
        }
    }
}

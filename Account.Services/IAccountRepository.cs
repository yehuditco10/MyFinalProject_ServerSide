using Account.Services.Models;
using System;
using System.Threading.Tasks;

namespace Account.Services
{
    public interface IAccountRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> CreateAccountAsync(Customer customer);
        Task<Customer> GetCustomerAsync(string email);
        Task<Models.Account> GetAccountAsync(Guid accountId);
        Task<Guid> GetAccountIdByCustomerIdAsync(Guid customerId);
        Task<bool> VerifyEmail(EmailVerification verification);
        Task<bool> CreateEmailVerificationAsync(EmailVerification emailVerification);
        Task<EmailVerification> GetVerificationDetails(string email);
        Task<bool> UpdateVerificationCodeAsync(string email, int verificationCode);
    }
}

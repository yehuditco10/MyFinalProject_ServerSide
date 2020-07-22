using Account.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Services
{
    public interface IAccountService
    {
        Task<bool> CreateAsync(Customer customer);
        Task<Guid> LoginAsync(string email, string password);
        Task<Models.Account> GetAccountAsync(Guid accountId);
        Task<bool> VerifyEmail(EmailVerification verificationModel);
        Task SendVerificationCodeAsync(string email);
        //?
        //string ToHash(string password);
    }
}

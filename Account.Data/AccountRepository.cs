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
                Entities.Customer newCustomer = _mapper.Map<Entities.Customer>(customerModel);
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

        public async Task<bool> CreateEmailVerificationAsync(Services.Models.EmailVerification emailVerificationModel)
        {
            try
            {
                Entities.EmailVerification emailVerification = _mapper.Map<Entities.EmailVerification>(emailVerificationModel);
                await _accountContext.EmailVerificationS.AddAsync(emailVerification);
                await _accountContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new EmailVerificationException(e.Message);
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

        public async Task<bool> VerifyEmail(Services.Models.EmailVerification verification)
        {
            try
            {
                var emailVerification = await _accountContext.EmailVerificationS.FirstOrDefaultAsync(c => c.Email == verification.Email);
                if (emailVerification == null)
                {
                    throw new EmailVerificationException("Email not found");
                }
                if (emailVerification.VerificationCode != verification.VerificationCode)
                    throw new EmailVerificationException("The verification code is wrong");
                if (emailVerification.ExpirationTime < DateTime.Now)
                    throw new EmailVerificationException("The expiration date has expired");
                var active = await ActivateAccount(verification.Email);
                if (active == -1)
                    throw new EmailVerificationException("Activate account failed");
                return true;
            }
            catch (Exception e)
            {
                throw new EmailVerificationException(e.Message);
            }

        }
        private async Task<int> ActivateAccount(string email)
        {
            try
            {
                var account = await _accountContext.Customers.FirstOrDefaultAsync(a => a.Email == email);
                if (account == null)
                    throw new AccountNotFoundException($"There is no accountt for {email}");
                account.Active = true;
                return await _accountContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //whice type?
                throw new Exception(e.Message);
            }
        }



        public async Task<Services.Models.EmailVerification> GetVerificationDetails(string email)
        {
            try
            {
                Entities.EmailVerification emailVerification = await _accountContext.EmailVerificationS.FirstOrDefaultAsync(
                              e => e.Email == email);
                if (emailVerification == null)
                    throw new AccountNotFoundException("we didn't find this email");//which exception
                return _mapper.Map<Services.Models.EmailVerification>(emailVerification);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<bool> UpdateVerificationCodeAsync(string email,int verificationCode)
        {
            try
            {
                EmailVerification emailVerification = await _accountContext.EmailVerificationS.FirstOrDefaultAsync(
                               e => e.Email == email);
                if(emailVerification==null)
                    throw new AccountNotFoundException("we didn't find this email");//which exception
                emailVerification.VerificationCode = verificationCode;
                //take the minutes number fron config file
                emailVerification.ExpirationTime = DateTime.Now.AddMinutes(60);
                await _accountContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }


            throw new NotImplementedException();
        }
    }
}

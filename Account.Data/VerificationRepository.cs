using Account.Services;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Account.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using Account.Data.Entities;

namespace Account.Data
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;

        public VerificationRepository(AccountContext accountContext,
            IMapper mapper)
        {
            _accountContext = accountContext;
            _mapper = mapper;
        }
        public async Task<Services.Models.EmailVerification> CreateEmailVerificationAsync(Services.Models.EmailVerification emailVerificationModel)
        {
            try
            {
                Entities.EmailVerification emailVerification = _mapper.Map<Entities.EmailVerification>(emailVerificationModel);
                await _accountContext.EmailVerificationS.AddAsync(emailVerification);
                await _accountContext.SaveChangesAsync();
                return _mapper.Map<Services.Models.EmailVerification>(emailVerification);
            }
            catch (Exception e)
            {
                throw new EmailVerificationException(e.Message);
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

        //put it here or in accountService?
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

        public async Task<bool> UpdateVerificationCodeAsync(string email, int verificationCode)
        {
            try
            {
                EmailVerification emailVerification = await _accountContext.EmailVerificationS.FirstOrDefaultAsync(
                               e => e.Email == email);
                if (emailVerification == null)
                    throw new AccountNotFoundException("we didn't find this email");//which exception
                emailVerification.VerificationCode = verificationCode;
                //take the minutes number fron config file
                emailVerification.ExpirationTime = DateTime.Now.AddMinutes(60);
                await _accountContext.SaveChangesAsync();
                //?? how to return(to ask?)
                return true;
            }
            catch (Exception)
            {
                //todo
                throw new EmailVerificationException("..");
            }
        }
    }
}

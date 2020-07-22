using Account.Services.Exceptions;
using Account.Services.Models;
using System;
using System.Security.Cryptography;
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
                //customer.Password = ToHash(customer.Password);
                var CreateAccountSucceded = await _accountRepository.CreateAccountAsync(customer);
                if (CreateAccountSucceded != -1)
                {
                    return true;
                }
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
            bool isValid = VerifyHashedPassword(customer.Password, password);
            if (isValid == false)
                throw new LoginFailedException("Your password is not valid");
            return await _accountRepository.GetAccountIdByCustomerIdAsync(customer.Id);
        }
        //private string ToHash(string password)
        //{
        //    byte[] salt;
        //    byte[] buffer2;
        //    if (password == null)
        //    {
        //        throw new ArgumentNullException("password");
        //    }
        //    using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
        //    {
        //        salt = bytes.Salt;
        //        buffer2 = bytes.GetBytes(0x20);
        //    }
        //    byte[] dst = new byte[0x31];
        //    Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
        //    Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
        //    return Convert.ToBase64String(dst);
        //}
        private static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            //byte[] buffer4;
            //if (hashedPassword == null)
            //{
            //    return false;
            //}
            //if (password == null)
            //{
            //    throw new ArgumentNullException("password");
            //}
            //byte[] src = Convert.FromBase64String(hashedPassword);
            //if ((src.Length != 0x31) || (src[0] != 0))
            //{
            //    return false;
            //}
            //byte[] dst = new byte[0x10];
            //Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            //byte[] buffer3 = new byte[0x20];
            //Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            //using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            //{
            //    buffer4 = bytes.GetBytes(0x20);
            //}
            ////return ByteArraysEqual(buffer3, buffer4);
            //check if them equal
            return true;
        }
    }
}

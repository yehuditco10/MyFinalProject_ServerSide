using Account.Services.Exceptions;
using Account.Services.Models;
using System;
using System.Net.Mail;
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

        public async Task SendVerificationCodeAsync(string email)
        {
            string subject = "Verification Code - Brix ";
            int code = GenerateRandomNo(1000, 9999);
            string body = $"Hello {email} </br>  your verify number is {code} </br><a href='http://localhost:4200/verification'>our site</a>";
            await SendEmail(email, subject, body);
        }
        private int GenerateRandomNo(int min,int max)
        {
            Random _rdm = new Random();
            return _rdm.Next(min, max);
        }
        private async Task SendEmail(string emailTo, string subject, string body)
        {
            try
            {
                string fromMail = "brixbootcamp@gmail.com";
                // string fromMail = ConfigurationManager.AppSettings["WeightWatcherEmailAddress"];
                string fromPassword = "brix2020";
                //string fromPassword = ConfigurationManager.AppSettings["WeightWatcherEmailAddress"];
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(fromMail);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential(fromMail, fromPassword);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

            }


            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<bool> VerifyEmail(EmailVerification verification)
        {
            return await _accountRepository.VerifyEmail(verification);
        }

        private bool VerifyHashedPassword( string customerPassword,string customerSaltPassword,string passwordFromUser)
        {
            if (Hashing.AreEqual(passwordFromUser, customerPassword, customerSaltPassword))
                return true;
            return false;
        }
    }
}

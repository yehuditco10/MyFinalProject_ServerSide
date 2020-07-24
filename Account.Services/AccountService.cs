using Account.Services.Exceptions;
using Account.Services.Models;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
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
                customer.PasswordHash = Hashing.GenerateHash(customer.Password, customer.PasswordSalt);
                var isCreated = await _accountRepository.CreateAccountAsync(customer);
                if (isCreated == false)
                    throw new Exception("Creation failed");
                EmailVerification emailVerification = new EmailVerification()
                {
                    Email = customer.Email,
                    VerificationCode = GenerateRandomNo(1000, 9999),
                    ExpirationTime = DateTime.Now.AddMinutes(60)
                };
                return await _accountRepository.CreateEmailVerificationAsync(emailVerification);
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
            bool isValid = VerifyHashedPassword(customer.PasswordHash, customer.PasswordSalt, password);
            if (isValid == false)
                throw new LoginFailedException("Your password is not valid");
            return await _accountRepository.GetAccountIdByCustomerIdAsync(customer.Id);
        }

        public async Task SendVerificationCodeAsync(string email)
        {
            EmailVerification verificationEmail = await _accountRepository.GetVerificationDetails(email);
            int verificationCode;
            if (verificationEmail.ExpirationTime <= DateTime.Now)
            {
                verificationCode = GenerateRandomNo(1000, 9999);
                bool succeeded = await _accountRepository.UpdateVerificationCodeAsync(email, verificationCode);
            }
            else
                verificationCode = verificationEmail.VerificationCode;

            string subject = "Verification Code - Brix ";
            //string subject = ConfigurationManager.AppSettings["VerificationEmailSubject"]; 

            // string body = $"Hello {email} </br>  your verify number is {code} </br><a href='http://localhost:4200/verification'>our site</a>";
            //string body = @"Hello " + email + "</br>  your verify number is " + code + "</br><a href='http://localhost:4200/verification'>our site</a>";
            string htmlText = @"
                    <head> 
                        <style> 
                            body{background-color:cadetblue;direction:rtl;text-align:center;}
                            h1,h3,p{font-size:20px; text-align:center;color:blue;}
                        </style>
                    </head>
                    <body>";
            htmlText += "<h1> hello " + email + "  </h1>" +
                "<p>" + " your verify number is " + verificationCode + " </p>" +
                " </ br >< a href = 'http://localhost:4200/verification' > our site </ a > " +
                "</body>";
            await SendEmail(email, subject, htmlText);
        }
        private int GenerateRandomNo(int min, int max)
        {
            Random _rdm = new Random();
            return _rdm.Next(min, max);
        }
        private async Task SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                string fromMail = "brixbootcamp@gmail.com";
                //string fromMail = ConfigurationManager.AppSettings["BrixEmailAddress"];
                string fromPassword = "brix2020";
                //string fromPassword = ConfigurationManager.AppSettings["BrixEmailPassword"];
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress(fromMail);
                mail.To.Add(toEmail);
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

        private bool VerifyHashedPassword(string customerPassword, string customerSaltPassword, string passwordFromUser)
        {
            if (Hashing.AreEqual(passwordFromUser, customerPassword, customerSaltPassword))
                return true;
            return false;
        }
    }
}

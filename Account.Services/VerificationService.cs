using Account.Services.Models;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Account.Services
{
   public class VerificationService:IVerificationService
    {
        private readonly IAccountService _accountService;
        private readonly IVerificationRepository _verificationRepository;

        //to inject the account servive? in order to get the random func?
        public VerificationService(
            //IAccountService accountService,
            IVerificationRepository verificationRepository)
        {
            //_accountService = accountService;
            _verificationRepository = verificationRepository;
        }
        public async Task SendVerificationCodeAsync(EmailVerification emailVerification)
        {
            int verificationCode;
            if (emailVerification.ExpirationTime <= DateTime.Now)
            {
                verificationCode = _accountService.GenerateRandomNo(1000, 9999);
                bool succeeded = await _verificationRepository.UpdateVerificationCodeAsync(emailVerification.Email, verificationCode);
            }
            else
                verificationCode = emailVerification.VerificationCode;

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
            htmlText += "<h1> hello " + emailVerification.Email + "  </h1>" +
                "<p>" + " your verify number is " + verificationCode + " </p>" +
                " </ br >< a href = 'http://localhost:4200/verification' > our site </ a > " +
                "</body>";
            await SendEmail(emailVerification.Email, subject, htmlText);
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
            return await _verificationRepository.VerifyEmail(verification);
        }

        // return ?
        public async Task ReSendVerificationCodeAsync(string email)
        {
            EmailVerification verificationEmail = await _verificationRepository.GetVerificationDetails(email);
            await SendVerificationCodeAsync(verificationEmail);
            //if resend has to send another code' we have to change the fubction UpdateVerificationCodeAsync
            //or to crate another one
        }
    }
}

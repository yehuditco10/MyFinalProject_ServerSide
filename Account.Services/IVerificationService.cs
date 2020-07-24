using Account.Services.Models;
using System.Threading.Tasks;

namespace Account.Services
{
   public interface IVerificationService
    {
        Task<bool> VerifyEmail(EmailVerification verificationModel);
        Task SendVerificationCodeAsync(EmailVerification emailVerification);
        Task ReSendVerificationCodeAsync(string email);
    }
}

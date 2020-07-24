using Account.Services.Models;
using System.Threading.Tasks;

namespace Account.Services
{
    public interface IVerificationRepository
    {
        Task<bool> VerifyEmail(EmailVerification verification);
        Task<EmailVerification> CreateEmailVerificationAsync(EmailVerification emailVerification);
        Task<EmailVerification> GetVerificationDetails(string email);
        Task<bool> UpdateVerificationCodeAsync(string email, int verificationCode);
    }
}

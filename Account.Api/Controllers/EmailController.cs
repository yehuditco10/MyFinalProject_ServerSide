using System.Threading.Tasks;
using Account.Api.DTO;
using Account.Services;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class EmailController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IVerificationService _verificationService;
        private readonly IMapper _mapper;
        public EmailController(IAccountService accountService,
            IVerificationService verificationService,
               IMapper mapper)
        {
            _accountService = accountService;
            _verificationService = verificationService;
            _mapper = mapper;
        }
        [HttpPost("resend/{email}")]
        public async Task<ActionResult> ReSendVerificationCodeAsync(string email)
        {
            await _verificationService.ReSendVerificationCodeAsync(email);
            return Ok();
        }
        [HttpPost("verification")]
        public async Task<ActionResult<bool>> EmailVerificationAsync(EmailVerification verification)
        {
            var verificationModel = _mapper.Map<Services.Models.EmailVerification>(verification);
            return await _verificationService.VerifyEmail(verificationModel);          
        }
    }
   
}
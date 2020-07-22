using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IMapper _mapper;
        public EmailController(IAccountService accountService,
               IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [HttpPost("{email}")]
        public async Task<ActionResult> SendVerificationCodeAsync(string email)
        {
            await _accountService.SendVerificationCodeAsync(email);
            return Ok();
        }
        [HttpPost("verification")]
        public async Task<ActionResult<bool>> EmailVerificationAsync(EmailVerification verification)
        {
            var verificationModel = _mapper.Map<Services.Models.EmailVerification>(verification);
            return await _accountService.VerifyEmail(verificationModel);
            //verificationModel.ExpirationTime = DateTime.Now.AddMinutes(30);
          
        }
        
    }
   
}
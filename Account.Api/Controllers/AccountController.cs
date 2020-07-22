using System;
using System.Threading.Tasks;
using Account.Api.DTO;
using Account.Services;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService,
               IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task<IActionResult> GetInfoAsync(Guid accountId)
        {
            Services.Models.Account account = await _accountService.GetAccountAsync(accountId);
            if (account != null)
            {
                AccountInfo accountInfo = new AccountInfo()
                {
                    FirstName = account.Customer.FirstName,
                    LastName = account.Customer.LastName,
                    Balance = account.Balance,
                    Opendate = account.Opendate
                };
                return Ok(accountInfo);
            }
            return NotFound();
        }
    }
}
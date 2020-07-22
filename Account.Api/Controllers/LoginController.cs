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
    public class LoginController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public LoginController(IAccountService accountService,
               IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<bool>> CreateAccountAsync(Customer customerDTO)
        {
            var customer = _mapper.Map<Services.Models.Customer>(customerDTO);
            return await _accountService.CreateAsync(customer);
        }
        [HttpGet]
        public async Task<IActionResult> LoginAsync([FromQuery]Login loginCustomer)
        {
            Guid accountId = await _accountService.LoginAsync(loginCustomer.Email, loginCustomer.Password);
            if (accountId != Guid.Empty)
            {
                return Ok(accountId);
            }
            return Unauthorized();
        }
    }
}
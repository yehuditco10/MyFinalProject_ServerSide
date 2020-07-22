using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Api.DTO
{
    public class Login
    {
        [Email]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

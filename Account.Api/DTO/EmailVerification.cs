using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Api.DTO
{
    public class EmailVerification
    {
        public string Email { get; set; }
        public int VerificationCode { get; set; }
    }
}

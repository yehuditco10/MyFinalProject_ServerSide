using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Services.Models
{
    public class EmailVerification
    {
        public string Email { get; set; }
        public int VerificationCode { get; set; }
    }
}

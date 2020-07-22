using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Api.DTO
{
    public class AccountInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Opendate { get; set; }
        public float Balance { get; set; }
    }
}

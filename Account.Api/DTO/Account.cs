using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Api.DTO
{
    public class Account
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Opendate { get; set; }
        public float Balance { get; set; }
        public virtual Customer Customer { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Services.Models
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

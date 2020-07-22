using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Opendate { get; set; }
        public int Balance { get; set; }
        public virtual Customer Customer { get; set; }
    }
}

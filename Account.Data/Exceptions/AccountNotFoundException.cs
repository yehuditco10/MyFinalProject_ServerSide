using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Exceptions
{
  public  class AccountNotFoundException:Exception
    {
        public AccountNotFoundException()
        {
        }

        public AccountNotFoundException(string message)
            : base(message)
        {
        }

        public AccountNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

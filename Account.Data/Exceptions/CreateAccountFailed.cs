using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Exceptions
{
   public class CreateAccountFailed:Exception
    {
        public CreateAccountFailed()
        {
        }

        public CreateAccountFailed(string message)
            : base(message)
        {
        }

        public CreateAccountFailed(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

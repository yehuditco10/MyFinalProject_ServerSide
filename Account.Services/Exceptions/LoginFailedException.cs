using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Services.Exceptions
{
    public class LoginFailedException:Exception
    {
        public LoginFailedException()
        {
        }

        public LoginFailedException(string message)
            : base(message)
        {
        }

        public LoginFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Exceptions
{
    public class EmailVerificationException : Exception
    {
        public EmailVerificationException()
        {
        }

        public EmailVerificationException(string message)
            : base(message)
        {
        }

        public EmailVerificationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

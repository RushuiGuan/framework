using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Authentication.Core
{
    public class InactiveUserException : Exception
    {
        public InactiveUserException(string user) : base($"User {user} is not active") { }
    }
}

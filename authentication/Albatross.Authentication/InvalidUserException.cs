using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Authentication {
	public class InvalidUserException : Exception {
		public InvalidUserException(string user) : base($"{user} is not a valid user") { }
	}
}
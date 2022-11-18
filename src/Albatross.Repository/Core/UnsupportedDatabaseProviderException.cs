using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.Core {
	public class UnsupportedDatabaseProviderException : Exception {
		public UnsupportedDatabaseProviderException(string provider) : base($"Database Provider {provider} is not supported") {
		}
	}
}

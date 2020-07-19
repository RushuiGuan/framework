using Albatross.Authentication.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Hosting.Test {
	public class GetCurrentTestUser : IGetCurrentUser {
		public string Provider { get; private set; }
		string account;

		public GetCurrentTestUser(string provider, string account) {
			this.Provider = provider;
			this.account = account;
		}

		public string Get() => account;
	}
}

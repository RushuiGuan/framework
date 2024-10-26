using Albatross.Authentication;

namespace Albatross.Hosting.Test {
	public class GetCurrentTestUser : IGetCurrentUser {
		public string Provider { get; set; }
		public string User { get; set; }

		public GetCurrentTestUser(string provider, string user) {
			this.Provider = provider;
			this.User = user;
		}

		public string Get() => this.User;
	}
}
using Albatross.Authentication;

namespace Albatross.EFCore.Test {
	public class GetCurrentTestUser : IGetCurrentUser {
		public GetCurrentTestUser(string user) {
			this.User = user;
		}
		public string Provider => "test";
		public string Get() => this.User;
		public string User { get; set; }
	}
}
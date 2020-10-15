using System.Collections.Generic;

namespace Albatross.Authentication.Server {
	public class User {
		public string Account { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }

		public IEnumerable<Group> Groups { get; set;  }
	}
}

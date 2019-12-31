using msg = Albatross.CRM.Messages;

namespace Albatross.CRM.Model {
	public class User {
		protected User() { }
		public User(int userID, string name) {
			UserID = userID;
			Name = name;
		}
		public int UserID {get;private set;}
		public string Name {get;private set;}
	}
}
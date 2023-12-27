namespace Sample.Messaging.Commands {
	public class MyBadCommand {
		public string Name { get; set; }
		public string Description { get; set; }

		/// <summary>
		///  this command cannot be deserialized property and will cause an error
		/// </summary>
		/// <param name="name"></param>
		/// <param name="myDescription"></param>
		public MyBadCommand(string name, string myDescription) {
			this.Name = name;
			this.Description = myDescription;
		}
	}
}

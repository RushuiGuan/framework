using Sample.Core.Commands.MyOwnNameSpace;

namespace Sample.Core.Commands {
	public class MyBadCommand : ISystemCommand {
		public string Name { get; set; }
		public string Description { get; set; }

		/// <summary>
		///  this command cannot be deserialized property and will cause an error
		/// </summary>
		/// <param name="name"></param>
		/// <param name="myDescription"></param>
		public MyBadCommand(string name, string myDescription) {
			Name = name;
			Description = myDescription;
		}
	}
}

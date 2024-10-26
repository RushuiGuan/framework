using Sample.Core.Commands.MyOwnNameSpace;

namespace Sample.Core.Commands {
	/// <summary>
	///  this command cannot be deserialized property and will cause an error
	/// </summary>
	public class SerializationErrorTestCommand : ISystemCommand {
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Callback { get; set; }

		public SerializationErrorTestCommand(string name, string myDescription) {
			Name = name;
			Description = myDescription;
		}
	}
}
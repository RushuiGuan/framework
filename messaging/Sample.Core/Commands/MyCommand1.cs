using Albatross.Messaging.Core;
using Sample.Core.Commands.MyOwnNameSpace;
using System.Collections.Generic;

namespace Sample.Core.Commands {
	[CommandName("a-custom-command-name")]
	[AlternateCommandName("b-custom-command-name")]
	public record class MyCommand1 : ISystemCommand {
		public MyCommand1(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public int Delay { get; set; }
		public bool Error { get; set; }
		public bool Callback => true;
		public List<TestOperationWithResultCommand> Commands { get; set; } = new List<TestOperationWithResultCommand>();
	}
}
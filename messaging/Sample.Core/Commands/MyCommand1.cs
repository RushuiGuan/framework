using Sample.Core.Commands.MyOwnNameSpace;
using System.Collections.Generic;

namespace Sample.Core.Commands {
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
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
		public List<MyCommand2> Commands { get; set; } = new List<MyCommand2>();
	}


	public record class MyCommand2 : ISystemCommand {
		public MyCommand2(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public int Delay { get; set; }
		public bool Error { get; set; }
		public bool Callback => true;
		public List<MyCommand1> Commands { get; set; } = new List<MyCommand1>();
	}

	public record class MyResult : ISystemCommand {
		public MyResult(string name) {
			Name = name;
		}
		public bool Callback => true;
		public string Name { get; set; }
	}
}
using Albatross.Messaging.Commands;
using System.Collections.Generic;

namespace SampleProject.Commands {
	[Command]
	public record class MyCommand1  {
		public MyCommand1(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public int Delay { get; set; }
		public bool Error { get; set; }
		public List<MyCommand2> Commands { get; set; } = new List<MyCommand2>();
	}

	[Command]
	public record class MyCommand2 {
		public MyCommand2(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public int Delay { get; set; }
		public bool Error { get; set; }
		public List<MyCommand1> Commands { get; set; } = new List<MyCommand1>();
	}

	[Command(typeof(MyResult))]
	public record class MyCommand3 {
		public MyCommand3(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public int Delay { get; set; }
		public bool Error { get; set; }
		public List<MyCommand1> Commands { get; set; } = new List<MyCommand1>();
	}

	public record class MyResult {
		public MyResult(string name) {
			Name = name;
		}
		public string Name { get; set; }
	}

	[Command]
	public record class SelfDestructCommand {
		public long Tick { get; set; }
		public int Delay { get; set; }
	}
}
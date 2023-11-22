using Albatross.Messaging.Commands;
using System.Collections.Generic;

namespace SampleProject.Commands {
	[Command]
	public record class MyCommand1  {
		public int Delay { get; }
		public string? Text { get; set; }
		public bool Error { get; set; }

		public List<MyCommand2> Commands { get; set; } = new List<MyCommand2>();
	}

	[Command]
	public record class MyCommand2 {
		public int Delay { get; }
		public string? Text { get; set; }
		public bool Error { get; set; }

		public List<MyCommand1> Commands { get; set; } = new List<MyCommand1>();
	}
}
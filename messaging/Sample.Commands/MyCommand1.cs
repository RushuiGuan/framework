using System.Collections.Generic;

namespace Sample.Commands {
	
	public record class MyCommand1  {
		public MyCommand1(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public int Delay { get; set; }
		public bool Error { get; set; }
		public List<MyCommand2> Commands { get; set; } = new List<MyCommand2>();
	}

	
	public record class MyCommand2 {
		public MyCommand2(string name) {
			Name = name;
		}

		public string Name { get; set; }
		public int Delay { get; set; }
		public bool Error { get; set; }
		public List<MyCommand1> Commands { get; set; } = new List<MyCommand1>();
	}

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

	
	public record class SelfDestructCommand {
		public long Tick { get; set; }
		public int Delay { get; set; }
	}
}
using System.CommandLine;

namespace Sample.CommandLine {
	public class MyCommand : Command {
		public MyCommand() : base("test", "my test command") {
		}
	}
}

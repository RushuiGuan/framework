using Albatross.CommandLine;

using System.CommandLine;
using System.IO;

namespace Sample.CommandLine {
	[Verb("yellow-command", typeof(YellowCommandHandler), "a sample yellow command", Alias = ["y"])]
	[Verb("red-command", typeof(RedCommandHandler), "a sample red command", Alias = ["r"])]
	public record class MyOptions {
		[Option(Description = "use the name of the test", Alias = ["n"])]
		public string Name { get; set; } = string.Empty;

		public int Data { get; set; }

		[Option(Description = "second file name", Alias = ["s"])]
		public FileInfo SecondFile { get; set; } = null!;


		[Option(Description = "input file name", Alias = ["x"])]
		public FileInfo MyFile { get; set; } = null!;
	}
}

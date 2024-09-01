using Albatross.CommandLine;
using System.CommandLine;

namespace Sample.CommandLine {
	[Verb("sys-command", typeof(SysCommandHandler), Alias = ["t"])]
	public record class SysCommandOptions {
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;

		[Option(Required = false)]
		public decimal Price { get; set; }

		[Option(Skip = true)]
		public int ShouldSkip { get; set; }

		[Option(Skip = false)]
		public int ShouldNotSkip { get; set; }

		[Option(Required = true)]
		public int? ForceRequired{ get; set; }
	}
}

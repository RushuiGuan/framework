using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Sample.CommandLine {
	// parent1 has two subcommands sub1 and sub2,  parent1 itself is not declared and will
	// be generated with a default HelpCommandHandler
	[Verb("parent1 sub1")]
	public class MySub1Options { }
	[Verb("parent1 sub2")]
	public class MySub2Options { }

	// parent2 has two subcommands sub1 and sub2,  parent2 itself is declared with a custom
	// handler.  All three commands should work independently.
	// notice that the sub commands names are only unique within the same parent.
	[Verb("parent2 sub1")]
	public class MySub3Options { }
	[Verb("parent2 sub2")]
	public class MySub4Options { }

	[Verb("parent2", typeof(MyParent2Handler))]
	public class MyParent2Options { }
	public class MyParent2Handler : BaseHandler<MyParent2Options> {
		public MyParent2Handler(IOptions<MyParent2Options> options) : base(options) {
		}
	}

	// parent3 has a subcommand sub1 and they share the same handler.  The sub command options
	// inherits from the parent options.  In this situation, the sub command in a sense extends
	// the parent command with additional options
	[Verb("parent3", typeof(MyParent3Handler))]
	public class MyParent3Options {
		public int Id { get; set; }
	}
	[Verb("parent3 sub1", typeof(MyParent3Handler))]
	public class MySub5Options : MyParent3Options {
		public string Name { get; set; } = string.Empty;
	}

	public class MyParent3Handler : BaseHandler<MySub5Options> {
		public MyParent3Handler(IOptions<MySub5Options> options) : base(options) { }
		public override int Invoke(InvocationContext context) {
			// remove base.Invoke and do some work here
			base.Invoke(context);
			return 0;
		}
	}
}

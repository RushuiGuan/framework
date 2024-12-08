using Albatross.CommandLine;
using Microsoft.Extensions.Options;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Sample.CommandLine {
	// parent1 has two subcommands sub1 and sub2,  parent1 itself is not declared and will
	// be generated with a default HelpCommandHandler
	[Verb("parent1 sub1", Description = "This command demonstrate the creation of a sub command without its parent command declaration")]
	public class MyParent1Sub1Options { }
	[Verb("parent1 sub2", Description = "This command demonstrate the creation of a sub command without its parent command declaration")]
	public class MyParent1Sub2Options { }

	public partial class Parent1Command : IRequireInitialization {
		public void Init() {
		}
	}

	// parent2 has two subcommands sub1 and sub2,  parent2 itself is declared with a custom
	// handler.  All three commands should work independently.
	// notice that the sub commands names are only unique within the same parent.
	[Verb("parent2 sub1", Description = "This demonstrate the explicit declaration of parent and sub command options")]
	public class MyParent2Sub1Options { }
	[Verb("parent2 sub2", Description = "This demonstrate the explicit declaration of parent and sub command options")]
	public class MyParent2Sub2Options { }

	[Verb("parent2", typeof(MyParent2Handler), Description = "This demonstrate the explicit declaration of parent and sub command options")]
	public class MyParent2Options { }
	public class MyParent2Handler : BaseHandler<MyParent2Options> {
		public MyParent2Handler(IOptions<MyParent2Options> options) : base(options) {
		}
	}

	// parent3 has a subcommand sub1 and they share the same handler.  The sub command options
	// inherits from the parent options.  In this situation, the sub command in a sense extends
	// the parent command with additional options
	[Verb("parent3", typeof(MyParent3Handler), Description ="This command shares the same handler and option class as its sub command")]
	public class MyParent3Options {
		public int Id { get; set; }
	}
	[Verb("parent3 sub1", typeof(MyParent3Handler), Description ="This command shares the same handler and base option class as its parent command")]
	public class MyParent3Sub1Options : MyParent3Options {
		public string Name { get; set; } = string.Empty;
	}

	public class MyParent3Handler : BaseHandler<MyParent3Sub1Options> {
		public MyParent3Handler(IOptions<MyParent3Sub1Options> options) : base(options) { }
		public override int Invoke(InvocationContext context) {
			// remove base.Invoke and do some work here
			base.Invoke(context);
			return 0;
		}
	}
}

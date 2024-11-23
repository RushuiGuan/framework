using Albatross.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using System.CommandLine;

namespace Sample.CommandLine {
	[Verb("parent1 sub1")]
	public class MySub1Options { }
	[Verb("parent1 sub2")]
	public class MySub2Options { }

	[Verb("parent2 sub1")]
	public class MySub3Options { }
	[Verb("parent2 sub2")]
	public class MySub4Options { }

	[Verb("parent2", typeof(MyParent2Handler))]
	public class MyParent2Options {
	}
	public class MyParent2Handler : ICommandHandler {
		public int Invoke(InvocationContext context) {
			context.Console.WriteLine("I am called");
			return 0;
		}
		public Task<int> InvokeAsync(InvocationContext context) {
			return Task.FromResult(Invoke(context));
		}
	}
}

using System.CommandLine.Invocation;

namespace Albatross.CommandLine {
	public static class Extensions {
		public static string ParsedCommandName(this InvocationContext context) => context.ParseResult.CommandResult.Command.Name;
	}
}

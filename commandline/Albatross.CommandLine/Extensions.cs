using System;
using System.CommandLine.Invocation;

namespace Albatross.CommandLine {
	public static class Extensions {
		public static string ParsedCommandName(this InvocationContext context) => context.ParseResult.CommandResult.Command.Name;

		public static string? Prompt(this string message) {
			Console.Write(message);
			return Console.ReadLine();
		}
	}
}

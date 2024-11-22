using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Albatross.CommandLine {
	public static class Extensions {
		public static string ParsedCommandName(this InvocationContext context) => context.ParseResult.CommandResult.Command.Name;

		public static string? Prompt(this string message) {
			Console.Write(message);
			return Console.ReadLine();
		}

		public static Command AddSubCommand<T>(this Command command) where T : Command, new() {
			var subCommand = new T();
			if(subCommand is IRequireInitialization instance) {
				instance.Init();
			}
			command.AddCommand(subCommand);
			return subCommand;
		}
	}
}
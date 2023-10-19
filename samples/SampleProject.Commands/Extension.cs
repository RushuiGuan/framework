using Albatross.Messaging.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SampleProject.Commands {
	public static class Extensions {
		static string GetQueueName(object command, IServiceProvider provider) {
			switch (command) {
				case KickOffDoNothingCommand kickoff:
					return $"kickoff - {kickoff.Id}";
				case DoNothingCommand doNothing:
					return $"donothing - {doNothing.Id % 10}";
				case DoMathWorkCommand:
				case ProcessDataCommand:
					return "math-queue";
				case LongRunningCommand:
				case UnstableCommand:
					return "show-running-queue";
				case FireAndForgetCommand:
					return "fire-and-forget-queue";
				case PingCommand:
				case PongCommand:
					return "ping pong";
			}
			return Albatross.Messaging.Commands.Extensions.DefaultQueueName;
		}
		public static IServiceCollection AddSampleProjectCommands(this IServiceCollection services) 
			=> services.AddAssemblyCommands(typeof(SampleProject.Commands.Extensions).Assembly, GetQueueName);
	}
}
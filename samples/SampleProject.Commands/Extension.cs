using Albatross.Messaging.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SampleProject.Commands {
	public static class Extensions {
		static string GetQueueName(object command, IServiceProvider provider) {
			switch (command) {
				case MyCommand1:
					return "my-command-queue1";
				case MyCommand2:
					return "my-command-queue2";
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
using System;

namespace Albatross.Commands {
	public interface IRegisterCommand{
		Type CommandType { get; }
		Type QueueType { get; }
		string GetQueueName(Command command);
	}

	public class RegisterCommand<T, Q> : IRegisterCommand where T : Command where Q : ICommandQueue {
		private readonly IServiceProvider provider;
		private readonly Func<IServiceProvider, Command, string> getQueueName;

		public Type CommandType => typeof(T);
		public Type QueueType => typeof(Q);


		public RegisterCommand(IServiceProvider provider, Func<IServiceProvider, Command, string> getQueueName) {
			this.provider = provider;
			this.getQueueName = getQueueName;
		}

		public string GetQueueName(Command command) {
			return getQueueName(this.provider, command);
		}
	}
}

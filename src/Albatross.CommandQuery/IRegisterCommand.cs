using System;

namespace Albatross.CommandQuery {
	public interface IRegisterCommand{
		Type CommandType { get; }
		Type QueueType { get; }
		string GetQueueName(Command command);
	}

	public class RegisterCommand<T, Q> : IRegisterCommand where T : Command where Q : ICommandQueue {
		private readonly Func<Command, string> getQueueName;

		public Type CommandType => typeof(T);
		public Type QueueType => typeof(Q);


		public RegisterCommand(Func<Command, string> getQueueName) {
			this.getQueueName = getQueueName;
		}

		public string GetQueueName(Command command) {
			return getQueueName(command);
		}
	}
}

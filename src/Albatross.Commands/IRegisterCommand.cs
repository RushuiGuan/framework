using System;

namespace Albatross.Commands {
	public interface IRegisterCommand{
		Type CommandType { get; }
		Type QueueType { get; }
		string GetQueueName(Command command, IServiceProvider provider);
		ICommandQueue Create(string name, IServiceProvider provider);
	}
	public interface IRegisterCommand<C, Q> : IRegisterCommand where C:Command where Q : ICommandQueue { 
	}

	public class RegisterCommand<C, Q> : IRegisterCommand<C, Q> where C : Command where Q : ICommandQueue {
		private readonly Func<C, IServiceProvider, string> getQueueName;
		private readonly Func<string, IServiceProvider, ICommandQueue> createQueue;

		public Type CommandType => typeof(C);
		public Type QueueType => typeof(Q);

		public RegisterCommand(Func<C, IServiceProvider, string> getQueueName, Func<string, IServiceProvider, ICommandQueue> createQueue) {
			this.getQueueName = getQueueName;
			this.createQueue = createQueue;
		}

		public string GetQueueName(Command command, IServiceProvider provider) {
			if (command is C) {
				return this.getQueueName((C)command, provider);
			} else {
				throw new ArgumentException();
			}
		}

		public ICommandQueue Create(string name, IServiceProvider provider) => this.createQueue(name, provider);
	}
}

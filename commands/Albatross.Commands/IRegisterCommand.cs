using System;

namespace Albatross.Commands {
	public interface IRegisterCommand{
		Type CommandType { get; }
		// Type QueueType { get; }
		string GetQueueName(Command command, IServiceProvider provider);
		ICommandQueue Create(string name, IServiceProvider provider);
	}
	public interface IRegisterCommand<C> : IRegisterCommand where C:Command { 
	}

	public class RegisterCommand<C> : IRegisterCommand<C> where C : Command  {
		private readonly Func<C, IServiceProvider, string> getQueueName;
		private readonly Func<string, IServiceProvider, ICommandQueue> createQueue;

		public Type CommandType => typeof(C);
		// public Type QueueType => typeof(Q);

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

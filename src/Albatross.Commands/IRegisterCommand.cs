using System;

namespace Albatross.Commands {
	public interface IRegisterCommand{
		Type CommandType { get; }
		Type QueueType { get; }
		string GetQueueName(Command command);
	}
	public interface IRegisterCommand<C, Q> : IRegisterCommand where C:Command where Q : ICommandQueue { }

	public class RegisterCommand<C, Q> : IRegisterCommand<C, Q> where C : Command where Q : ICommandQueue {
		private readonly IServiceProvider provider;
		private readonly Func<IServiceProvider, C, string> getQueueName;

		public Type CommandType => typeof(C);
		public Type QueueType => typeof(Q);


		public RegisterCommand(IServiceProvider provider, Func<IServiceProvider, C, string> getQueueName) {
			this.provider = provider;
			this.getQueueName = getQueueName;
		}

		public string GetQueueName(C command) {
			return getQueueName(this.provider, command);
		}

		string IRegisterCommand.GetQueueName(Command command) {
			if(command is C) {
				return GetQueueName((C)command);
			} else {
				throw new ArgumentException();
			}
		}
	}
}

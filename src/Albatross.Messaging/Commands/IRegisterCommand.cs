using System;
namespace Albatross.Messaging.Commands {
	public interface IRegisterCommand {
		bool HasReturnType { get; }
		Type CommandType { get; }
		Type ResponseType { get; }
		Type CommandHandlerType { get; }
		string GetQueueName(object command, IServiceProvider provider);
	}
	public interface IRegisterCommand<T> : IRegisterCommand where T : ICommand { }

	public class RegisterCommand<T> : IRegisterCommand<T> where T : ICommand {
		private readonly Func<T, IServiceProvider, string> getQueueName;

		public Type CommandType => typeof(T);
		public Type ResponseType => T.ResponseType;
		public Type CommandHandlerType { get; init; }
		public bool HasReturnType => T.ResponseType != typeof(void);

		public RegisterCommand(Func<T, IServiceProvider, string> getQueueName) {
			this.getQueueName = getQueueName;
			if (ResponseType == typeof(void)) {
				CommandHandlerType = typeof(ICommandHandler<>).MakeGenericType(typeof(T));
			} else {
				CommandHandlerType = typeof(ICommandHandler<,>).MakeGenericType(typeof(T), T.ResponseType);
			}
		}

		public string GetQueueName(object obj, IServiceProvider provider) {
			if (obj is T command) {
				return this.getQueueName(command, provider);
			} else {
				throw new ArgumentException();
			}
		}
	}
}

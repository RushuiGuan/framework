using System;
namespace Albatross.Messaging.Commands {
	public interface IRegisterCommand {
		bool HasReturnType { get; }
		Type CommandType { get; }
		Type ResponseType { get; }
		Type CommandHandlerType { get; }
		string GetQueueName(object command, IServiceProvider provider);
	}
	public interface IRegisterCommand<T> : IRegisterCommand where T : notnull { }
	public interface IRegisterCommand<T, K> : IRegisterCommand where T : notnull where K:notnull { }

	public class RegisterCommand : IRegisterCommand {
		private readonly Func<object, IServiceProvider, string> getQueueName;
		public bool HasReturnType => ResponseType != typeof(void);
		public Type CommandType { get; init; }
		public Type ResponseType { get; init; }
		public Type CommandHandlerType {get;init; }

		public string GetQueueName(object command, IServiceProvider provider) => this.getQueueName(command, provider);
		public RegisterCommand(Type commandType, Type responseType, Func<object, IServiceProvider, string> getQueueName) {
			this.CommandType = commandType;
			this.ResponseType = responseType;
			if (responseType == typeof(void)) {
				this.CommandHandlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
			}else {
				this.CommandHandlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, responseType);
			}
			this.getQueueName = getQueueName;
		}
	}

	public class RegisterCommand<T> : IRegisterCommand where T : notnull {
		private readonly Func<T, IServiceProvider, string> getQueueName;
		
		public bool HasReturnType => false;
		public Type CommandType => typeof(T);
		public Type ResponseType => typeof(void);
		public Type CommandHandlerType => typeof(ICommandHandler<T>);

		public RegisterCommand(Func<T, IServiceProvider, string> getQueueName) {
			this.getQueueName = getQueueName;
		}

		public string GetQueueName(object obj, IServiceProvider provider) {
			if (obj is T command) {
				return this.getQueueName(command, provider);
			} else {
				throw new ArgumentException();
			}
		}
	}

	public class RegisterCommand<T, K> : IRegisterCommand where T : notnull where K:notnull {
		private readonly Func<T, IServiceProvider, string> getQueueName;
		
		public bool HasReturnType => true;
		public Type CommandType => typeof(T);
		public Type ResponseType => typeof(K);
		public Type CommandHandlerType => typeof(ICommandHandler<T, K>);

		public RegisterCommand(Func<T, IServiceProvider, string> getQueueName) {
			this.getQueueName = getQueueName;
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

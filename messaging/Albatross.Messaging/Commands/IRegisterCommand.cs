﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
namespace Albatross.Messaging.Commands {
	public interface IRegisterCommand {
		bool HasReturnType { get; }
		IEnumerable<string> Alias { get; }
		Type CommandType { get; }
		Type ResponseType { get; }
		string GetQueueName(object command, IServiceProvider provider);
		ICommandHandler CreateCommandHandler(IServiceProvider provider);
	}
	public interface IRegisterCommand<T> : IRegisterCommand where T : notnull { }
	public interface IRegisterCommand<T, K> : IRegisterCommand where T : notnull where K : notnull { }

	public class RegisterCommand<T> : IRegisterCommand<T> where T : notnull {
		private readonly Func<T, IServiceProvider, string> getQueueName;

		public bool HasReturnType => false;
		public IEnumerable<string> Alias { get; }
		public Type CommandType => typeof(T);
		public Type ResponseType => typeof(void);
		public Type CommandHandlerType => typeof(ICommandHandler<T>);

		public RegisterCommand(string[] alias, Func<T, IServiceProvider, string> getQueueName) {
			this.Alias = alias;
			this.getQueueName = getQueueName;
		}

		public string GetQueueName(object obj, IServiceProvider provider) {
			if (obj is T command) {
				return this.getQueueName(command, provider);
			} else {
				throw new ArgumentException();
			}
		}

		public ICommandHandler CreateCommandHandler(IServiceProvider provider)
			=> provider.GetRequiredService<ICommandHandler<T>>();
	}

	public class RegisterCommand<T, K> : IRegisterCommand<T, K> where T : notnull where K : notnull {
		private readonly Func<T, IServiceProvider, string> getQueueName;

		public IEnumerable<string> Alias { get; }
		public bool HasReturnType => true;
		public Type CommandType => typeof(T);
		public Type ResponseType => typeof(K);

		public RegisterCommand(string[] alias, Func<T, IServiceProvider, string> getQueueName) {
			this.Alias = alias;
			this.getQueueName = getQueueName;
		}

		public string GetQueueName(object obj, IServiceProvider provider) {
			if (obj is T command) {
				return this.getQueueName(command, provider);
			} else {
				throw new ArgumentException();
			}
		}

		public ICommandHandler CreateCommandHandler(IServiceProvider provider)
			=> provider.GetRequiredService<ICommandHandler<T, K>>();
	}
}

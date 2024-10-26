using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	public abstract class BaseCommandHandler<CommandType, ResponseType> : ICommandHandler<CommandType, ResponseType>
		where CommandType : notnull where ResponseType : notnull {
		public abstract Task<ResponseType> Handle(CommandType command);
		public async Task<object> Handle(object obj) {
			if (obj is CommandType command) {
				ResponseType result = await this.Handle(command);
				return result;
			} else {
				throw new ArgumentException($"Invalid command type:{obj.GetType().FullName}");
			}
		}
	}

	public abstract class BaseCommandHandler<CommandType> : ICommandHandler<CommandType> where CommandType : notnull {
		private readonly static object any = new object();
		public abstract Task Handle(CommandType command);
		public async Task<object> Handle(object obj) {
			if (obj is CommandType command) {
				await this.Handle(command);
				return any;
			} else {
				throw new ArgumentException($"Invalid command type:{obj.GetType().FullName}");
			}
		}
	}
}
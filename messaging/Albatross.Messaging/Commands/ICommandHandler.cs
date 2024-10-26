using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Commands {
	// command handlers should be registered as scoped
	public interface ICommandHandler {
		Task<object> Handle(object command);
	}
	public interface ICommandHandler<CommandType, ResponseType> : ICommandHandler
		where CommandType : notnull
		where ResponseType : notnull {

		Task<ResponseType> Handle(CommandType command);
	}
	public interface ICommandHandler<CommandType> : ICommandHandler
		where CommandType : notnull {
		Task Handle(CommandType command);
	}
}
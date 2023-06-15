using System;

namespace Albatross.Messaging.Commands {
	public interface ICommand { 
		static abstract Type ResponseType { get; }
	}
	public class Command<ResponseType> : ICommand  where ResponseType: notnull {
		static Type ICommand.ResponseType => typeof(ResponseType);
	}

	public class Command : ICommand {
		static Type ICommand.ResponseType => typeof(void);
	}
}

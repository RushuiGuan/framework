using Albatross.Messaging.Core;

namespace Sample.Core.Commands.MyOwnNameSpace {
	[CommandInterface]
	public partial interface ISystemCommand {
		bool Callback { get; }
	}
}
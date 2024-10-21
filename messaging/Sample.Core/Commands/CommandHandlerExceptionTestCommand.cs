using Sample.Core.Commands.MyOwnNameSpace;

namespace Sample.Core.Commands {
	public record class CommandHandlerExceptionTestCommand : ISystemCommand {
		public int Delay { get; set; }
		public bool Callback { get; set; }
	}
}
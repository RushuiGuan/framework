using Sample.Core.Commands.MyOwnNameSpace;

namespace Sample.Core.Commands {
	/// <summary>
	/// this command will kill the daemon process
	/// </summary>
	public record class SelfDestructCommand : ISystemCommand {
		public bool Callback => false;
		public int? Delay { get; set; }
	}
}
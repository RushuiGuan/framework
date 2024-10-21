using Sample.Core.Commands.MyOwnNameSpace;

namespace Sample.Core.Commands {
	public record class TestOperationWithResultCommand : ISystemCommand {
		public TestOperationWithResultCommand(string name) {
			Name = name;
		}

		public bool Callback { get; set; }
		public int Value { get; set; }
		public string Name { get; set; }
	}
}
namespace Test.Dto.Classes {
	public class Command1 : ICommand {
		public string Name { get; set; } = string.Empty;
	}
	public class Command2 : IEmptyInterface {
		public string Name { get; set; } = string.Empty;
	}
}

namespace Sample.CommandLine {
	public record class BaseCommandOptions {
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}

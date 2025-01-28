namespace Albatross.CodeGen.WebClient.Settings {
	public record class WebClientConstructorSettings {
		public bool Omit { get; init; }
		public string? CustomJsonSettings { get; init; }
	}
}
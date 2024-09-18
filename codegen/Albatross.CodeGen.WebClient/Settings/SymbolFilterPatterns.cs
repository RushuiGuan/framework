namespace Albatross.CodeGen.WebClient.Settings {
	/// <summary>
	/// Note that exclusion pattern is applied first
	/// </summary>
	public record class SymbolFilterPatterns {
		public string? Exclusion { get; init; }
		public string? Inclusion { get; init; }

		public SymbolFilterPatterns Overwrite(SymbolFilterPatterns other) {
			return new SymbolFilterPatterns {
				Exclusion = Exclusion ?? other.Exclusion,
				Inclusion = Inclusion ?? other.Inclusion,
			};
		}
	}
}

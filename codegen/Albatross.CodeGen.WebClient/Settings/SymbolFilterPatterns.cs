namespace Albatross.CodeGen.WebClient.Settings {
	/// <summary>
	/// Note that exclusion pattern is applied first
	/// </summary>
	public record class SymbolFilterPatterns {
		public string? Exclude { get; init; }
		public string? Include { get; init; }

		public SymbolFilterPatterns Overwrite(SymbolFilterPatterns other) {
			return new SymbolFilterPatterns {
				Exclude = Exclude ?? other.Exclude,
				Include = Include ?? other.Include,
			};
		}
	}
}

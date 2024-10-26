namespace Albatross.CodeGen.WebClient.Settings {
	/// <summary>
	/// Note that exclusion pattern is applied first
	/// </summary>
	public record class SymbolFilterPatterns {
		public string? Exclude { get; init; }
		public string? Include { get; init; }

		public SymbolFilterPatterns Overwrite(SymbolFilterPatterns other) {
			if (HasValue) {
				return this;
			} else {
				return other;
			}
		}

		public bool HasValue => !string.IsNullOrWhiteSpace(Exclude) || !string.IsNullOrWhiteSpace(Include);
	}
}
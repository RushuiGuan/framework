namespace Sample.EFCore {
	public record class JsonProperty {
		public string? Text { get; }
		public JsonProperty(string? text) {
			Text = text;
		}
	}
}
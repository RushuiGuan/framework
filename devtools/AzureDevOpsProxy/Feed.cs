namespace AzureDevOpsProxy {
	public record class Feed {
		public string? Description { get; set; }
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public bool IsEnabled { get; set; }
		public bool IsReadOnly { get; set; }
		public string Url { get; set; } = string.Empty;
	}
}
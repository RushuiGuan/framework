namespace AzureDevOpsProxy {
	public record class PackageVersion {
		public string Id { get; set; } = string.Empty;
		public string NormalizedVersion { get; set; } = string.Empty;
		public string Version { get; set; } = string.Empty;
		public bool IsLatest { get; set; }
		public bool IsListed { get; set; }
		public string StorageId { get; set; } = string.Empty;
		public DateTime PublishDate { get; set; }
		public string? Url { get; set; } = string.Empty;
	}
}

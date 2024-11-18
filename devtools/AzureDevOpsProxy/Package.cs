namespace AzureDevOpsProxy {
	public record class Package {
		public string Id { get; set; } = string.Empty;
		public string NormalizedName { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string ProtocolType { get; set; } = string.Empty;
		public string Url { get; set; } = string.Empty;
		public PackageVersion[] Versions { get; set; } = Array.Empty<PackageVersion>();
	}
}

namespace AzureDevOpsProxy {
	public record class ItemCollection<T> {
		public int Count { get; set; }
		public T[] Value { get; set; } = Array.Empty<T>();
	}
}

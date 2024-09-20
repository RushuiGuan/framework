namespace Test.Dto {
	public record class CollectionValueType {
		public IEnumerable<int> IntCollection { get; set; } = [];
		public IEnumerable<int?> NullableIntCollection { get; set; } = [];
	}
}

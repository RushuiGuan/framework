namespace Test.Dto.Classes {
	public record class CollectionValueType {
		public IEnumerable<int> IntCollection { get; set; } = [];
		public IEnumerable<int?> NullableIntCollection { get; set; } = [];
	}
}
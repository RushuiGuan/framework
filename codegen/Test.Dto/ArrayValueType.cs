namespace Test.Dto {
	public record class ArrayValueType {
		public int[] IntArray { get; set; } = [];
		public int?[] NullableIntArray { get; set; } = [];
	}
}

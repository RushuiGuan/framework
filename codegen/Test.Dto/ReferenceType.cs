namespace Test.Dto {
	public class ReferenceType {
		public string String { get; set; } = string.Empty;
		public MyClassWithGenericBaseType MyClass { get; set; } = null!;

		public string? NullableString { get; set; }
		public MyClassWithGenericBaseType? NullableMyClass { get; set; }
	}
}

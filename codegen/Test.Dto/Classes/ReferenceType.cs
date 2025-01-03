using System.Text.Json;

namespace Test.Dto.Classes {
	public class ReferenceType {
		public string String { get; set; } = string.Empty;
		public object Object { get; set; } = null!;
		public MyClassWithGenericBaseType MyClass { get; set; } = null!;

		public string? NullableString { get; set; }
		public MyClassWithGenericBaseType? NullableMyClass { get; set; }
		public object? NullableObject { get; set; }
	}
}
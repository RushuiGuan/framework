using System.Text.Json.Serialization;

namespace Test.Dto.Enums {
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum MyStringEnum {
		One = 1,
		Two = 2,
		Three = 3
	}
}
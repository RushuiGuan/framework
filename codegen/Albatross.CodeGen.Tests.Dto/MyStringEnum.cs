using System.Text.Json.Serialization;

namespace Albatross.CodeGen.Tests.Dto {
	[JsonConverter(typeof(JsonStringEnumConverter<MyStringEnum>))]
	public enum MyStringEnum {
		One = 1,
		Two = 2,
		Three = 3
	}
}

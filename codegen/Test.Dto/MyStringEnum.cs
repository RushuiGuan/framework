﻿using System.Text.Json.Serialization;

namespace Test.Dto {
	[JsonConverter(typeof(JsonStringEnumConverter<MyStringEnum>))]
	public enum MyStringEnum {
		One = 1,
		Two = 2,
		Three = 3
	}
}
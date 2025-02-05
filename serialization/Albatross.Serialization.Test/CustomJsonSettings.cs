using System;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Albatross.Serialization.Test {
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum SecurityTypeText {
		Undefined  = -1,
		Equity = 1,
		FixedIncome = 2
	}

	public enum SecurityTypeInt {
		Undefined = -1,
		Equity = 1,
		FixedIncome = 2
	}
	public class CustomJsonSettings : IJsonSettings {
		public JsonSerializerOptions Default { get; private set; }
		public JsonSerializerOptions Alternate => throw new NotSupportedException();
		public CustomJsonSettings() {
			Default = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				Converters = {
					new StringEnumConverterWithFallback<SecurityTypeText>(SecurityTypeText.Undefined),
				}
			};
		}
		readonly static Lazy<CustomJsonSettings> lazy = new Lazy<CustomJsonSettings>();
		public static CustomJsonSettings Value => lazy.Value;
	}
}
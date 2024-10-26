using System;
using System.Text.Json;

namespace Albatross.EFCore {
	public class EFCoreJsonOption : Albatross.Serialization.IJsonSettings {
		public static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
			Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() },
		};

		public JsonSerializerOptions Default => DefaultOptions;

		public JsonSerializerOptions Alternate => throw new NotSupportedException();
	}
}
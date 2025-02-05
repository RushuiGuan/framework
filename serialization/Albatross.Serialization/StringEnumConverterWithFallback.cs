using System;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Albatross.Serialization {
	/// <summary>
	/// This converter is useful to provided backward compatibility when the enum value is not found.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class StringEnumConverterWithFallback<T> : JsonConverter<T> where T : struct {
		private readonly T fallback;

		public StringEnumConverterWithFallback(T fallback) {
			this.fallback = fallback;
		}
		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			if (reader.TokenType == JsonTokenType.String) {
				if (Enum.TryParse<T>(reader.GetString(), true, out T result)) {
					return result;
				}
			}
			return fallback;
		}
		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
			writer.WriteStringValue(value.ToString());
		}
	}
}

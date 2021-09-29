using System;
using System.Text.Json;
using System.Buffers;
using System.Data;

namespace Albatross.Serialization {
	public static class Extension {
		public static T? ToObject<T>(this JsonElement element, JsonSerializerOptions? options = null) {
			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				element.WriteTo(writer);
			}
			return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
		}

		public static object? ToObject(this JsonElement element, Type type, JsonSerializerOptions? options = null) {

			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				element.WriteTo(writer);
			}
			return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, type, options);
		}

		public static JsonElement ToJsonElement<T>(this T t, JsonSerializerOptions? options =null) {
			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				JsonSerializer.Serialize<T>(writer, t, options);
			}
			return JsonSerializer.Deserialize<JsonElement>(bufferWriter.WrittenSpan, options);
		}

		public static void ApplyJsonValue(Utf8JsonWriter writer, JsonElement src, JsonElement value, JsonSerializerOptions? options = null) {
			if (value.ValueKind == JsonValueKind.Undefined) {
				JsonSerializer.Serialize<JsonElement>(writer, src, options);
			} else if (src.ValueKind == JsonValueKind.Object && value.ValueKind == JsonValueKind.Object) {
				writer.WriteStartObject();
				foreach (var property in src.EnumerateObject()) {
					writer.WritePropertyName(options?.PropertyNamingPolicy?.ConvertName(property.Name) ?? property.Name);
					if (value.TryGetProperty(property.Name, out JsonElement overrideProperty)) {
						ApplyJsonValue(writer, property.Value, overrideProperty, options);
					} else {
						JsonSerializer.Serialize<JsonElement>(writer, property.Value, options);
					}
				}
				foreach (var property in value.EnumerateObject()) {
					if (!src.TryGetProperty(property.Name, out _)) {
						writer.WritePropertyName(property.Name);
						JsonSerializer.Serialize<JsonElement>(writer, property.Value, options);
					}
				}
				writer.WriteEndObject();
			} else {
				JsonSerializer.Serialize<JsonElement>(writer, value, options);
			}
		}

		/// <summary>
		/// Use src as the base and overwrite its properties with input.  The property path has to match.
		/// </summary>
		/// <param name="src">base json</param>
		/// <param name="input">json to be applied</param>
		/// <param name="options"></param>
		/// <returns>The result json element</returns>
		public static JsonElement ApplyJsonValue(JsonElement src, JsonElement input, JsonSerializerOptions? options = null) {
			ArrayBufferWriter<byte> bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				ApplyJsonValue(writer, src, input, options);
			}
			return JsonSerializer.Deserialize<JsonElement>(bufferWriter.WrittenSpan, options);
		}
		
		public static void WriteJson(this IDataReader reader, Utf8JsonWriter writer, JsonSerializerOptions? options = null) {
			writer.WriteStartArray();
			while (reader.Read()) {
				writer.WriteStartObject();
				for (int i = 0; i < reader.FieldCount; i++) {
					writer.WritePropertyName(reader.GetName(i));
					object value = reader.GetValue(i);
					if (value == DBNull.Value || value == null) {
						writer.WriteNullValue();
					} else {
						JsonSerializer.Serialize(writer, value, value.GetType(), options);
					}
				}
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}
		//public static bool TryGetJsonPropertyValue(this JsonElement elem, string propertyName, Type type, out object propertyValue) {
		//	if (elem.ValueKind == JsonValueKind.Null || elem.ValueKind == JsonValueKind.Undefined) {
		//		propertyValue = new object();
		//		return false;
		//	}
		//	if (elem.TryGetProperty(propertyName, out JsonElement value)) {
		//		propertyValue = value.ToObject(type);
		//		return true;
		//	} else {
		//		propertyValue = new object();
		//		return false;
		//	}
		//}



		public static JsonElement WriteJson(this IDataReader reader, JsonSerializerOptions options = null) {
			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				reader.WriteJson(writer, options);
			}
			return JsonSerializer.Deserialize<JsonElement>(bufferWriter.WrittenSpan, options);
		}

		//public static dynamic? Convert(this JsonElement elem) {
		//	switch (elem.ValueKind) {
		//		case JsonValueKind.True:
		//			return true;
		//		case JsonValueKind.False:
		//			return false;
		//		case JsonValueKind.String:
		//			string? text = elem.GetString();
		//			if (DateTime.TryParse(text, out DateTime dateTime)) {
		//				return dateTime;
		//			} else {
		//				return text;
		//			}
		//		case JsonValueKind.Number:
		//			return elem.TryGetInt64(out long value) ? value : elem.GetDouble();
		//		case JsonValueKind.Null:
		//		case JsonValueKind.Undefined:
		//			return null;
		//		case JsonValueKind.Object:
		//			IDictionary<string, object> expando = new ExpandoObject();
		//			foreach (var child in elem.EnumerateObject()) {
		//				expando.Add(child.Name, Convert(child.Value));
		//			}
		//			return expando;
		//		default:
		//			return elem;
		//	}
		//}



		//public static bool CheckPropertyName(this JsonSerializerOptions? options, string expected, string actual) {
		//	if (options?.PropertyNamingPolicy == null) {
		//		return expected == actual;
		//	} else if (options?.PropertyNameCaseInsensitive == true) {
		//		return string.Equals(expected, actual, StringComparison.InvariantCultureIgnoreCase);
		//	} else {
		//		return options?.PropertyNamingPolicy?.ConvertName(expected) == actual;
		//	}
		//}

		//public static string GetPropertyName(this JsonSerializerOptions options, string name) => options?.PropertyNamingPolicy?.ConvertName(name) ?? name;

	}
}

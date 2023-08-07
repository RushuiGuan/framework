using System;
using System.Text.Json;
using System.Buffers;
using System.Data;
using System.Text.Json.Serialization;

namespace Albatross.Serialization {
	public static class Extensions {
		static void ApplyJsonValue(Utf8JsonWriter writer, JsonElement src, JsonElement value, JsonSerializerOptions? options = null) {
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
			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				ApplyJsonValue(writer, src, input, options);
			}
			return JsonSerializer.Deserialize<JsonElement>(bufferWriter.WrittenSpan, options);
		}

		[Obsolete]
		public static void WriteJson(this IDataReader reader, Utf8JsonWriter writer, JsonSerializerOptions? options = null) {
			writer.WriteStartArray();
			while (reader.Read()) {
				reader.WriteJsonSingleRow(writer, options);
			}
			writer.WriteEndArray();
		}
		[Obsolete]
		public static void WriteJsonSingleRow(this IDataReader reader, Utf8JsonWriter writer, JsonSerializerOptions? options = null) {
			writer.WriteStartObject();
			for (int i = 0; i < reader.FieldCount; i++) {
				object value = reader.GetValue(i);
				if (value == DBNull.Value || value == null) {
					if (options?.DefaultIgnoreCondition != JsonIgnoreCondition.WhenWritingNull) { 
						writer.WriteNull(reader.GetName(i));
					}
				} else { 
					writer.WritePropertyName(reader.GetName(i));
					JsonSerializer.Serialize(writer, value, value.GetType(), options);
				}
			}
			writer.WriteEndObject();
		}
		[Obsolete]
		public static JsonElement WriteJson(this IDataReader reader, JsonSerializerOptions? options = null) {
			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				reader.WriteJson(writer, options);
			}
			return JsonSerializer.Deserialize<JsonElement>(bufferWriter.WrittenSpan, options);
		}
	}
}

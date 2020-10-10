using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Buffers;
using System.Dynamic;
using Albatross.Reflection;

namespace Albatross.Serialization {
	public static class Extension	{

		public static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null) {
			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				element.WriteTo(writer);
			}
			return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
		}

		public static object ToObject(this JsonElement element, Type type, JsonSerializerOptions options = null) {

			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				element.WriteTo(writer);
			}
			return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, type, options);
		}

		public static Dictionary<string, object> ToObject(this JsonElement element, Dictionary<string, Type> properties, Action<string, object> action = null) {
			Dictionary<string, object> result = new Dictionary<string, object>();
			if (element.ValueKind == JsonValueKind.Null || element.ValueKind == JsonValueKind.Undefined) {
				return result;
			} else if (element.ValueKind == JsonValueKind.Object) {
				foreach (var pair in properties) {
					if (element.TryGetProperty(pair.Key, out JsonElement value)) {
						object propertyValue = value.ToObject(pair.Value);
						action?.Invoke(pair.Key, propertyValue);
						result.Add(pair.Key, propertyValue);
					}
				}
				return result;
			} else {
				throw new ArgumentException($"Invalid json element: {element}");
			}
		}

		public static bool TryGetJsonPropertyValue(this JsonElement elem, string propertyName, Type type, out object propertyValue) {
			if (elem.ValueKind == JsonValueKind.Null || elem.ValueKind == JsonValueKind.Undefined) {
				propertyValue = null;
				return false;
			}
			if (elem.TryGetProperty(propertyName, out JsonElement value)) {
				propertyValue = value.ToObject(type);
				return true;
			} else {
				propertyValue = null;
				return false;
			}
		}

		public static void WriteJson(this IDataReader reader, Utf8JsonWriter writer, JsonSerializerOptions options = null) {
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

		public static JsonElement WriteJson(this IDataReader reader, JsonSerializerOptions options = null) {
			var bufferWriter = new ArrayBufferWriter<byte>();
			using (var writer = new Utf8JsonWriter(bufferWriter)) {
				reader.WriteJson(writer, options);
			}
			return JsonSerializer.Deserialize<JsonElement>(bufferWriter.WrittenSpan, options);
		}

		public static dynamic Convert(this JsonElement elem) {
			switch (elem.ValueKind) {
				case JsonValueKind.True:
					return true;
				case JsonValueKind.False:
					return false;
				case JsonValueKind.String: 
						string text= elem.GetString();
					if (DateTime.TryParse(text, out DateTime dateTime)) {
						return dateTime;
					} else {
						return text;
					}
				case JsonValueKind.Number:
					return elem.TryGetInt64(out long value) ? value : elem.GetDouble();
				case JsonValueKind.Null:
				case JsonValueKind.Undefined:
					return null;
				case JsonValueKind.Object:
					IDictionary<string, object> expando = new ExpandoObject();
					foreach(var child in elem.EnumerateObject()) {
						expando.Add(child.Name, Convert(child.Value));
					}
					return expando;
				default:
					return elem;
			}
		}

		public static string SerializeValue(this TypedValue value, out Type type, JsonSerializerOptions options = null) {
			type = value.ClassName.GetClass();
			return JsonSerializer.Serialize(value.Value, type, options);
		}

		public static object DeserializeValue(this TypedValue value, string text, JsonSerializerOptions options = null) {
			Type type = value.ClassName.GetClass();
			object result = JsonSerializer.Deserialize(text, type, options);
			value.Value = result;
			return result;
		}
	}
}

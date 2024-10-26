using Albatross.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Albatross.Serialization {
	public class JsonEnumTextConverter<T> : JsonConverter<T> where T : struct {
		Dictionary<string, Map> dict = new Dictionary<string, Map>();
		class Map {
			public T Value { get; }
			public string Text { get; }
			public Map(T value, string text) {
				Value = value;
				Text = text;
			}
		}
		public JsonEnumTextConverter() {
			var type = typeof(T);
			if (type.IsEnum) {
				foreach (T value in Enum.GetValues(type)) {
					var name = Enum.GetName(type, value);
					var attrib = value.GetEnumMemberAttribute<T, EnumTextAttribute>();
					var text = attrib?.Text ?? name;
					dict[name] = new Map(value, text);
					if (attrib != null) {
						dict[text] = new Map(value, text);
					}
				}
			} else {
				throw new ArgumentException($"{typeof(T)} is not an enum");
			}
		}

		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			Debug.Assert(typeToConvert == typeof(T));
			var text = reader.GetString() ?? throw new JsonException($"The JSON value could not be converted to {typeof(T)}");
			if (dict.TryGetValue(text, out var map)) {
				return map.Value;
			} else {
				throw new JsonException($"cannot convert text {text} to {typeof(T)}");
			}
		}

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
			var name = Enum.GetName(typeof(T), value);
			writer.WriteStringValue(dict[name].Text);
		}
	}
}
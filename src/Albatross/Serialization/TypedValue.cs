using Albatross.Reflection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Albatross.Serialization {
	[JsonConverter(typeof(TypedValueJsonConverter<TypedValue>))]
	public class TypedValue {
		public string ClassName { get; set; }
		public object Value { get; set; }
	}
	public class TypedValueJsonConverter<T> : JsonConverter<T> where T : TypedValue, new() {
		

		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			T t = new T();
			var startDepth = reader.CurrentDepth;
			JsonElement valueElement = new JsonElement();
			bool valueSet = false;
			bool classNameSet = false;
			while (reader.Read()) {
				if (reader.TokenType == JsonTokenType.EndObject && reader.CurrentDepth == startDepth) { break; }
				if (reader.TokenType == JsonTokenType.PropertyName) {
					string propertyName = reader.GetString();
					if (options.CheckPropertyName(nameof(TypedValue.ClassName), propertyName)) {
						if (reader.Read()) { 
							t.ClassName = reader.GetString();
							classNameSet = true;
						}
					}else if(options.CheckPropertyName(nameof(TypedValue.Value), propertyName)) {
						if (classNameSet) {
							Type type = t.ClassName.GetClass();
							t.Value = JsonSerializer.Deserialize(ref reader, type, options);
							valueSet = true;
						} else {
							valueElement = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
						}
					} else {
						ReadAdditionalProperty(ref reader, t, propertyName, options);
					}
				}
			}
			if (string.IsNullOrEmpty(t.ClassName)) {
				t.ClassName = typeof(JsonElement).GetTypeNameWithoutAssemblyVersion();
				t.Value = valueElement;
				valueSet = true;
			}
			if (!valueSet) {
				Type type = t.ClassName.GetClass();
				if (type == typeof(JsonElement)) {
					t.Value = valueElement;
				} else {
					if (valueElement.ValueKind == JsonValueKind.Undefined) {
						t.Value = null;
					} else {
						t.Value = JsonSerializer.Deserialize(valueElement.GetRawText(), type, options);
					}
				}
			}
			Validator.ValidateObject(t, new ValidationContext(t));
			return t;
		}

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
			writer.WriteStartObject();
			if (!string.IsNullOrEmpty(value.ClassName)) {
				writer.WriteString(options.GetPropertyName(nameof(TypedValue.ClassName)), value.ClassName);
			}else if (!options.IgnoreNullValues) {
				writer.WriteNull(options.GetPropertyName(nameof(TypedValue.ClassName)));
			}
			if (value.Value != null) {
				writer.WritePropertyName(options.GetPropertyName(nameof(TypedValue.Value)));
				JsonSerializer.Serialize(writer, value.Value, options);
			} else if (!options.IgnoreNullValues) {
				writer.WriteNull(options.GetPropertyName(nameof(TypedValue.Value)));
			}
			WriteAdditionalProperty(writer, value, options);
			writer.WriteEndObject();
		}
		protected virtual void ReadAdditionalProperty(ref Utf8JsonReader reader, T t, string propertyName, JsonSerializerOptions options) { }
		protected virtual void WriteAdditionalProperty(Utf8JsonWriter writer, T value, JsonSerializerOptions options) { }
	}
}

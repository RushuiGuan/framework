using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Albatross.Serialization
{
	public static class Extension
	{
		public static T ToObject<T>(this JsonElement element) {
			string text = JsonSerializer.Serialize(element);
			return JsonSerializer.Deserialize<T>(text);
		}

		public static object ToObject(this JsonElement element, Type type) {
			string text = JsonSerializer.Serialize(element);
			return JsonSerializer.Deserialize(text, type);
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
	}
}

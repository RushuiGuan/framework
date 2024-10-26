using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Albatross.Reflection {
	public static class Enumerations {
		static string GetPropertyKey(string? path, int? index, string? name) {
			string key;
			if (string.IsNullOrEmpty(path)) {
				return name ?? string.Empty;
			} else {
				if (!string.IsNullOrEmpty(name)) {
					name = $".{name}";
				}
				if (index.HasValue) {
					key = $"{path}[{index}]{name}";
				} else {
					key = $"{path}{name}";
				}
			}
			return key;
		}
		/// <summary>
		/// Flatten an object into a dictionary.  The key is the path to the property.  The value is the value of the property.
		/// If a property is an array, then the index with square brackets is appended to the path.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="path"></param>
		/// <param name="index"></param>
		/// <param name="result"></param>
		public static void Property(object? value, string? path, int? index, Dictionary<string, object> result) {
			if (value == null) {
				return;
			} else if (value is string) {
				result.Add(GetPropertyKey(path, index, null), value);
			} else if (value.GetType().IsValueType) {
				result.Add(GetPropertyKey(path, index, null), value);
			} else {
				var type = value.GetType();
				if (type.IsArray || typeof(IEnumerable).IsAssignableFrom(type)) {
					var newPath = GetPropertyKey(path, index, null);
					int newIndex = 0;
					foreach (var item in (IEnumerable)value) {
						Property(item, newPath, newIndex, result);
						newIndex++;
					}
				} else {
					foreach (var property in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
						var propertyValue = property.GetValue(value);
						Property(propertyValue, GetPropertyKey(path, index, property.Name), null, result);
					}
				}
			}
		}
	}
}
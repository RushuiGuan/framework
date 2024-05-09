using System;
using System.Collections.Generic;
using Albatross.Reflection;

namespace Albatross.Text {
	public static class ConversionExtensions {
		public static object? Convert(this string? text, Type type) {
			if (string.IsNullOrEmpty(text)) {
				if (type == typeof(string)) {
					return text;
				}else if (type.IsValueType) {
					return Activator.CreateInstance(type);
				} else {
					return null;
				}
			}
			switch (Type.GetTypeCode(type)) {
				case TypeCode.Int32: return int.Parse(text);
				case TypeCode.Int64: return long.Parse(text);
				case TypeCode.String: return text;
				case TypeCode.DateTime: return DateTime.Parse(text);
				case TypeCode.Decimal: return decimal.Parse(text);
				case TypeCode.Double: return double.Parse(text);
				case TypeCode.Boolean: return bool.Parse(text);
				case TypeCode.Char: return text[0];
			};
			if (type.IsEnum) {
				return Enum.Parse(type, text, true);
#if NET6_0_OR_GREATER
			} else if (type == typeof(DateOnly)) {
				return DateOnly.Parse(text);
#endif
			} else if (type.GetNullableValueType(out var valueType)) {
				return Convert(text, valueType);
			} else {
				throw new NotSupportedException();
			}
		}

		public static T Convert<T>(this IDictionary<string, string> dictionary) where T : new() {
			var t = new T();
			foreach (var keyValuePair in dictionary) {
				var property = typeof(T).GetProperty(keyValuePair.Key);
				if (property?.GetSetMethod() != null) {
					var value = keyValuePair.Value.Convert(property.PropertyType);
					property.SetValue(t, value);
				}
			}
			return t;
		}
	}
}

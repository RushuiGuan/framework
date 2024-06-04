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
			if(type.IsEnum && Enum.TryParse(type, text, true, out var value)) {
				return value;
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
			if (type.GetNullableValueType(out var valueType)) {
				return Convert(text, valueType);
#if NET6_0_OR_GREATER
			} else if (type == typeof(DateOnly)) {
				return DateOnly.Parse(text);
#endif
			} else {
				throw new NotSupportedException();
			}
		}

		public static T Convert<T>(this IDictionary<string, string> dictionary) where T : new() {
			var t = new T();
			foreach (var keyValuePair in dictionary) {
				var propertyType = typeof(T).GetPropertyType(keyValuePair.Key, true);
				var value = keyValuePair.Value.Convert(propertyType);
				typeof(T).SetPropertyValue(t, keyValuePair.Key, value, true);
			}
			return t;
		}
	}
}

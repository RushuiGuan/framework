using System;
using System.Collections.Generic;
using Albatross.Reflection;

namespace Albatross.Text {
	public static class ConversionExtensions {
		public static object? Convert(this string? text, Type type) {
			if (string.IsNullOrEmpty(text)) {
				if (type == typeof(string)) {
					return text;
				} else if (type.IsValueType) {
					return Activator.CreateInstance(type);
				} else {
					return null;
				}
			}
			if (type.IsEnum && Enum.TryParse(type, text, true, out var value)) {
				return value;
			}
			switch (Type.GetTypeCode(type)) {
				case TypeCode.SByte: return sbyte.Parse(text);
				case TypeCode.Byte: return byte.Parse(text);
				case TypeCode.Int16: return short.Parse(text);
				case TypeCode.UInt16: return ushort.Parse(text);
				case TypeCode.Int32: return int.Parse(text);
				case TypeCode.UInt32: return uint.Parse(text);
				case TypeCode.Int64: return long.Parse(text);
				case TypeCode.UInt64: return ulong.Parse(text);
				case TypeCode.String: return text;
				case TypeCode.DateTime: return DateTime.Parse(text);
				case TypeCode.Single: return float.Parse(text);
				case TypeCode.Double: return double.Parse(text);
				case TypeCode.Decimal: return decimal.Parse(text);
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
				throw new NotSupportedException($"Cannot convert text \"{text}\" to type {type.Name}");
			}
		}

		public static T Convert<T>(this IDictionary<string, string> dictionary, Func<T> func) where T : notnull {
			var obj = func();
			Convert(dictionary, typeof(T), obj);
			return obj;
		}

		public static object Convert(this IDictionary<string, string> dictionary, Type type, object obj) {
			foreach (var keyValuePair in dictionary) {
				var propertyType = type.GetPropertyType(keyValuePair.Key, true);
				var value = keyValuePair.Value.Convert(propertyType);
				type.SetPropertyValue(obj, keyValuePair.Key, value, true);
			}
			return obj;
		}
	}
}

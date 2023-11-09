using System;
using System.Reflection;

namespace Albatross.Reflection {
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class EnumTextAttribute : Attribute {
		public EnumTextAttribute(string text) {
			Text = text;
		}
		public string Text { get; }
	}

	public static class EnumTextExtensions {
		public static A? GetEnumMemberAttribute<EnumType, A>(this EnumType enumValue) where A : Attribute where EnumType : struct {
			var type = typeof(EnumType);
			if (type.IsEnum) {
				var value = enumValue.ToString();
				var members = type.GetMember(value);
				return members[0].GetCustomAttribute<A>();
			} else {
				throw new ArgumentException($"Type {type.FullName} is not an enum");
			}
		}
		public static string GetEnumText<EnumType>(this EnumType value) where EnumType : struct
			=> value.GetEnumMemberAttribute<EnumType, EnumTextAttribute>()?.Text 
				?? throw new ArgumentException($"{value} is missing the {typeof(EnumTextAttribute)}");
	}
}

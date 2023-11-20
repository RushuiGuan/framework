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
		public static A? GetEnumMemberAttribute<A>(Type type, object enumValue) where A : Attribute  {
			if (type.IsEnum) {
				var name = Enum.GetName(type, enumValue);
				var members = type.GetMember(name);
				return members[0].GetCustomAttribute<A>();
			} else {
				throw new ArgumentException($"Type {type.FullName} is not an enum");
			}
		}
		public static A? GetEnumMemberAttribute<EnumType, A>(this EnumType enumValue) where A : Attribute where EnumType : struct
			=> GetEnumMemberAttribute<A>(typeof(EnumType), enumValue);

		public static A GetRequiredEnumMemberAttribute<A>(Type type, object enumValue) where A : Attribute
			=> GetEnumMemberAttribute<A>(type, enumValue) ?? throw new ArgumentException($"{enumValue} is missing the {typeof(EnumTextAttribute)}");

		public static A GetRequiredEnumMemberAttribute<EnumType, A>(this EnumType enumValue) where A : Attribute where EnumType : struct
			=> GetRequiredEnumMemberAttribute<A>(typeof(EnumType), enumValue);

		public static string GetEnumText<EnumType>(this EnumType value) where EnumType : struct
			=> value.GetRequiredEnumMemberAttribute<EnumType, EnumTextAttribute>().Text;

		public static string GetEnumText(Type enumType, object value)
			=> GetRequiredEnumMemberAttribute<EnumTextAttribute>(enumType, value).Text;
	}
}

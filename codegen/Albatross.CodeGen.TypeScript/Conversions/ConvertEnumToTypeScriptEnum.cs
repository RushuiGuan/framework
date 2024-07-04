using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.TypeConversions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertEnumToTypeScriptEnum : IConvertObject<Type, EnumDeclaration> {
		private readonly TypeConverterFactory factory;

		public ConvertEnumToTypeScriptEnum(TypeConverterFactory factory) {
			this.factory = factory;
		}

		public EnumDeclaration Convert(Type type) {
			if (type.IsEnum) {
				var useStringValue = type.GetCustomAttribute<JsonConverterAttribute>()?.ConverterType == typeof(JsonStringEnumConverter);
				return new EnumDeclaration(type.Name) {
					Items = new ListOfEnumItems(GetEnumItems(type, useStringValue)),
				};
			} else {
				throw new InvalidOperationException($"Type {type.Name} is not an Enum type");
			}
		}
		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);

		IEnumerable<EnumItemExpression> GetEnumItems(Type enumType, bool useStringValue) {
			foreach (var value in enumType.GetEnumValues()) {
				var name = Enum.GetName(enumType, value) ?? throw new InvalidOperationException($"Enum value {value} does not have a name");
				LiteralExpression expression = useStringValue ? new StringLiteralExpression(name) : new NumberLiteralExpression((int)value);
				yield return new EnumItemExpression(name) {
					Expression = expression,
				};
			}
		}
	}
}

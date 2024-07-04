using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.Modifiers;
using Albatross.CodeGen.TypeScript.TypeConversions;
using System;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertTypeToTypeScriptInterface : IConvertObject<Type, InterfaceDeclaration> {
		ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty;
		private readonly ConvertTypeToTypeScriptType convertTypeToTypeScriptType;
		private readonly TypeConverterFactory factory;

		public ConvertTypeToTypeScriptInterface(ConvertPropertyInfoToTypeScriptProperty convertPropertyInfoToTypeScriptProperty, ConvertTypeToTypeScriptType convertTypeToTypeScriptType, TypeConverterFactory factory) {
			this.convertPropertyInfoToTypeScriptProperty = convertPropertyInfoToTypeScriptProperty;
			this.convertTypeToTypeScriptType = convertTypeToTypeScriptType;
			this.factory = factory;
		}
		public InterfaceDeclaration Convert(Type type) {
			return new InterfaceDeclaration(type.Name) {
				Properties = (from property in type.GetProperties(System.Reflection.BindingFlags.Public
							 | System.Reflection.BindingFlags.DeclaredOnly
							 | System.Reflection.BindingFlags.Instance)
							  select convertPropertyInfoToTypeScriptProperty.Convert(property)).ToList(),
				BaseInterfaceName = type.BaseType != null && type.BaseType != typeof(object) && !type.BaseType.IsValueType ? new IdentifierNameExpression(type.BaseType.Name) : null,
				Modifiers = [AccessModifier.Public],
			};
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}

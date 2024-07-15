using Albatross.CodeAnalysis;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Albatross.Text;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertPropertySymbolToTypeScriptProperty : IConvertObject<IPropertySymbol, PropertyDeclaration> {
		private readonly ITypeConverterFactory converterFactory;

		public ConvertPropertySymbolToTypeScriptProperty(ITypeConverterFactory converterFactory) {
			this.converterFactory = converterFactory;
		}

		public PropertyDeclaration Convert(IPropertySymbol from) {
			ITypeExpression type = converterFactory.Convert(from.Type);
			var optional = from.Type.IsNullable();
			if (optional) {
				var genericType = type as GenericTypeExpression;
				if (genericType != null) {
					type = genericType.Arguments.First();
				} else {
					throw new InvalidOperationException();
				}
			}
			return new PropertyDeclaration(from.Name.CamelCase()) {
				Type = type,
				Optional = optional,
			};
		}

		object IConvertObject<IPropertySymbol>.Convert(IPropertySymbol from)
			=>  this.Convert(from);
	}
}

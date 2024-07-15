using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System.Linq;
using Albatross.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertEnumToTypeScriptEnum : IConvertObject<INamedTypeSymbol, EnumDeclaration> {
		public EnumDeclaration Convert(INamedTypeSymbol from) {
			if (from.HasAttribute("System.Text.Json.Serialization.JsonConverterAttribute")) {
				return new EnumDeclaration(from.Name) {
					Items = new ListOfEnumItems(
						from.GetMembers().Where(x => x.Kind == SymbolKind.Field)
						.Select(x => new EnumItemExpression(x.Name) {
							Expression = new StringLiteralExpression(x.Name),
						})
					),
				};
			} else {
				return new EnumDeclaration(from.Name) {
					Items = new ListOfEnumItems(
						from.GetMembers().Where(x => x.Kind == SymbolKind.Field).OfType<IFieldSymbol>()
						.Select(x => new EnumItemExpression(x.Name) {
							Expression = new NumberLiteralExpression(x.ConstantValue!),
						}))
				};
			}
		}

		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) {
			return Convert(from);
		}
	}
}

using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertEnumToTypeScriptEnum2 : IConvertObject<INamedTypeSymbol, EnumDeclaration> {
		public EnumDeclaration Convert(INamedTypeSymbol from) {
			if (from.GetAttributes().Where(x => x.AttributeClass?.ToDisplayString() == "System.Text.Json.Serialization.JsonConverter").Any()) {
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
							Expression = new NumberLiteralExpression((int)x.ConstantValue!),
						}))
				};
			}
		}

		object IConvertObject<INamedTypeSymbol>.Convert(INamedTypeSymbol from) {
			return Convert(from);
		}
	}
}

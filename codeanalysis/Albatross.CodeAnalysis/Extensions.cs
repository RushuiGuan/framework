using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public static class Extensions {
		public static INamedTypeSymbol GetRequiredSymbol(this Compilation compilation, string typeName) {
			var symbol = compilation.GetTypeByMetadataName(typeName);
			if (symbol == null) {
				throw new ArgumentException($"Type {typeName} not found in compilation");
			}
			return symbol;
		}
		public static bool IsDerivedFrom(this ITypeSymbol typeSymbol, ITypeSymbol baseTypeSymbol) {
			for (var baseType = typeSymbol.BaseType; baseType != null; baseType = baseType.BaseType) {
				if (SymbolEqualityComparer.Default.Equals(baseType, baseTypeSymbol)) {
					return true;
				}
			}
			return false;
		}

		public static bool IsConstructedFrom(this INamedTypeSymbol typeSymbol, INamedTypeSymbol genericDefinitionSymbol) {
			if (typeSymbol.IsGenericType && genericDefinitionSymbol.IsGenericType && genericDefinitionSymbol.IsDefinition) {
				return SymbolEqualityComparer.Default.Equals(typeSymbol.OriginalDefinition, genericDefinitionSymbol);
			} else {
				return false;
			}
		}

		public static Compilation CreateCompilation(params string[] sourceCodes) {
			var syntaxTrees = sourceCodes.Select(code => CSharpSyntaxTree.ParseText(code)).ToArray();

			var references = new List<MetadataReference> {
				MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(System.Text.Json.JsonDocument).Assembly.Location),
			};

			var compilation = CSharpCompilation.Create(
				"TestCompilation",
				syntaxTrees,
				references,
				new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

			return compilation;
		}

		public static bool TryGetGenericTypeArguments(this ITypeSymbol symbol, string genericTypeDefinitionName, out ITypeSymbol[] arguments) {
			if (symbol is INamedTypeSymbol named && named.IsGenericType && named.OriginalDefinition.ToDisplayString() == genericTypeDefinitionName) {
				arguments = named.TypeArguments.ToArray();
				return true;
			} else {
				arguments = Array.Empty<ITypeSymbol>();
				return false;
			}
		}
	}
}

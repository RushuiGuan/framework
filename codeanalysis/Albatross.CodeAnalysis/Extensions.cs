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
			if (baseTypeSymbol.TypeKind == TypeKind.Interface) {
				return typeSymbol.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, baseTypeSymbol));
			} else if (typeSymbol.TypeKind == TypeKind.Class) {
				for (var baseType = typeSymbol.BaseType; baseType != null; baseType = baseType.BaseType) {
					if (SymbolEqualityComparer.Default.Equals(baseType, baseTypeSymbol)) {
						return true;
					}
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
			var syntaxTrees = sourceCodes.Select(code => CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.Default))).ToArray();

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

		public static bool IsNumeric(this ITypeSymbol symbol) {
			switch (symbol.SpecialType) {
				case SpecialType.System_Byte:
				case SpecialType.System_SByte:
				case SpecialType.System_Int16:
				case SpecialType.System_UInt16:
				case SpecialType.System_Int32:
				case SpecialType.System_UInt32:
				case SpecialType.System_Int64:
				case SpecialType.System_UInt64:
				case SpecialType.System_Single:
				case SpecialType.System_Double:
				case SpecialType.System_Decimal:
					return true;
				default:
					return false;
			}
		}

		public static bool HasAttribute(this ISymbol symbol, string attributeName) {
			const string AttributePostfix = "Attribute";
			foreach (var attribute in symbol.GetAttributes()) {
				var className = attribute.AttributeClass?.ToDisplayString();
				if (!string.IsNullOrEmpty(className)) {
					if (!className.EndsWith(AttributePostfix)) {
						className += AttributePostfix;
					}
					if(className == attributeName) {
						return true;
					}
				}
			}
			return false;
		}
		public static bool IsNullable(this ITypeSymbol symbol) {
			return symbol is INamedTypeSymbol named 
				&& named.IsGenericType 
				&& named.OriginalDefinition.ToDisplayString() == "System.Nullable<T>";}
	}
}

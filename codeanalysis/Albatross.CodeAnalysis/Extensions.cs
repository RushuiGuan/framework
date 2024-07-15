using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
			if (symbol is INamedTypeSymbol named && named.IsGenericType && named.OriginalDefinition.GetFullName() == genericTypeDefinitionName) {
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
			foreach (var attribute in symbol.GetAttributes()) {
				var className = attribute.AttributeClass?.GetFullName();
				if (!string.IsNullOrEmpty(className)) {
					if (className == attributeName) {
						return true;
					}
				}
			}
			return false;
		}

		public static bool IsNullable(this ITypeSymbol symbol) {
			return symbol is INamedTypeSymbol named
				&& named.IsGenericType
				&& named.OriginalDefinition.GetFullName() == "System.Nullable<>";
		}

		public static string GetFullName(this ITypeSymbol symbol) {
			string fullName;
			if (symbol is IArrayTypeSymbol arraySymbol) {
				fullName = $"{arraySymbol.ElementType.GetFullName()}[]";
			} else if (symbol.ContainingNamespace == null) {
				fullName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			} else if (symbol.ContainingNamespace.IsGlobalNamespace) {
				fullName = symbol.Name;
			} else {
				fullName = $"{symbol.ContainingNamespace.GetFullNamespace()}.{symbol.Name}";
			}
			if (symbol is INamedTypeSymbol named && named.IsGenericType) {
				var sb = new StringBuilder(fullName);
				sb.Append("<");
				for (int i = 0; i < named.TypeArguments.Length; i++) {
					if (i > 0) { 
						sb.Append(","); 
					}
					if (!named.IsDefinition) {
						sb.Append(named.TypeArguments[i].GetFullName());
					}
				}
				sb.Append(">");
				fullName = sb.ToString();
			}
			return fullName;
		}
		public static string GetFullNamespace(this INamespaceSymbol symbol) {
			if (symbol.IsGlobalNamespace) {
				return string.Empty;
			} else if (symbol.ContainingNamespace.IsGlobalNamespace) {
				return symbol.Name;
			} else {
				return $"{GetFullNamespace(symbol.ContainingNamespace)}.{symbol.Name}";
			}
		}
	}
}

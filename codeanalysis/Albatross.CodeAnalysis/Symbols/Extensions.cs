using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeAnalysis.Symbols {
	public static class Extensions {
		public static INamedTypeSymbol GetRequiredSymbol(this Compilation compilation, string typeName) {
			var symbol = compilation.GetTypeByMetadataName(typeName);
			if (symbol == null) {
				throw new ArgumentException($"Type {typeName} not found in compilation");
			}
			return symbol;
		}

		public static bool IsDerivedFrom(this ITypeSymbol typeSymbol, string baseTypeName) {
			if (typeSymbol.TypeKind == TypeKind.Class) {
				for (var baseType = typeSymbol.BaseType; baseType != null; baseType = baseType.BaseType) {
					if (baseType.GetFullName() == baseTypeName) {
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

		public static bool TryGetGenericTypeArguments(this ITypeSymbol symbol, string genericTypeDefinitionName, out ITypeSymbol[] arguments) {
			if (symbol is INamedTypeSymbol named && named.IsGenericType && named.OriginalDefinition.GetFullName() == genericTypeDefinitionName) {
				arguments = named.TypeArguments.ToArray();
				return true;
			} else {
				arguments = Array.Empty<ITypeSymbol>();
				return false;
			}
		}

		public static bool TryGetNullableValueType(this ITypeSymbol symbol, out ITypeSymbol? valueType) {
			if (symbol is INamedTypeSymbol named && named.IsGenericType && named.OriginalDefinition.GetFullName() == My.GenericDefinition.Nullable) {
				valueType = named.TypeArguments.Single();
				return true;
			} else {
				valueType = null;
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

		public static bool IsNullable(this ITypeSymbol symbol) => symbol is INamedTypeSymbol named
			&& (named.IsGenericType && named.OriginalDefinition.GetFullName() == My.GenericDefinition.Nullable || symbol.NullableAnnotation == NullableAnnotation.Annotated);

		public static bool IsNullableReferenceType(this ITypeSymbol symbol) => symbol is INamedTypeSymbol named && !named.IsValueType && symbol.NullableAnnotation == NullableAnnotation.Annotated;

		public static bool IsNullableValueType(this ITypeSymbol symbol) => symbol is INamedTypeSymbol named && named.IsGenericType && named.OriginalDefinition.GetFullName() == My.GenericDefinition.Nullable;

		/// <summary>
		/// for the sake of our sanity, this method will return false for string
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns></returns>
		public static bool IsCollection(this ITypeSymbol symbol) {
			if (symbol.SpecialType == SpecialType.System_String) {
				return false;
			} else if (symbol is IArrayTypeSymbol) {
				return true;
			} else {
				return symbol.GetFullName() == My.Class.IEnumerable || symbol.AllInterfaces.Any(x => x.GetFullName() == My.Class.IEnumerable);
			}
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
				return $"{symbol.ContainingNamespace.GetFullNamespace()}.{symbol.Name}";
			}
		}

		public static bool TryGetCollectionElementType(this ITypeSymbol typeSymbol, out ITypeSymbol? elementType) {
			if (typeSymbol.SpecialType == SpecialType.System_String) {
				elementType = null;
				return false;
			} else if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol) {
				elementType = arrayTypeSymbol.ElementType;
				return true;
			} else {
				if (typeSymbol.TryGetGenericTypeArguments(My.GenericDefinition.IEnumerable, out var arguments)) {
					elementType = arguments[0];
					return true;
				} else {
					var ienumerableDefinition = typeSymbol.AllInterfaces.FirstOrDefault(x => x.IsGenericType && x.OriginalDefinition.GetFullName() == My.GenericDefinition.IEnumerable);
					if (ienumerableDefinition != null) {
						elementType = ienumerableDefinition.TypeArguments[0];
						return true;
					} else {
						elementType = null;
						return false;
					}
				}
			}
		}

		public static bool IsGenericTypeDefinition(this INamedTypeSymbol symbol) => symbol.IsGenericType && symbol.IsDefinition;

		public static IEnumerable<IPropertySymbol> GetProperties(this INamedTypeSymbol symbol, bool useBaseClassProperties) {
			if (useBaseClassProperties && symbol.BaseType != null) {
				foreach (var member in GetProperties(symbol.BaseType, useBaseClassProperties)) {
					yield return member;
				}
			}
			foreach (var member in symbol.GetMembers().OfType<IPropertySymbol>()) {
				if (member.SetMethod?.DeclaredAccessibility == Accessibility.Public
					&& member.GetMethod?.DeclaredAccessibility == Accessibility.Public) {
					yield return member;
				}
			}
		}

		public static IEnumerable<IPropertySymbol> GetDistinctProperties(this INamedTypeSymbol symbol, bool useBaseClassProperties) {
			var set = new HashSet<string>();
			foreach (var item in symbol.GetProperties(useBaseClassProperties)) {
				if (set.Add(item.Name)) {
					yield return item;
				}
			}
		}

		public static bool IsPartial(this INamedTypeSymbol symbol) =>
			symbol.DeclaringSyntaxReferences.Select(x => x.GetSyntax())
				.OfType<InterfaceDeclarationSyntax>().Any(x => x.Modifiers.Any(SyntaxKind.PartialKeyword));

		public static string GetTypeNameRelativeToNamespace(this ITypeSymbol symbol, string currentNamespace) {
			var fullName = symbol.GetFullName();
			if (fullName.StartsWith(currentNamespace + ".")) {
				return fullName.Substring(currentNamespace.Length + 1);
			} else {
				return symbol.GetFullName();
			}
		}

		public static bool IsConcreteClass(this INamedTypeSymbol symbol) => 
			symbol.TypeKind == TypeKind.Class 
			&& !symbol.IsAbstract
			&& !symbol.IsStatic
			&& !symbol.IsGenericTypeDefinition();
	}
}
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Symbols {
	public static class AttributeDataExtensions {
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

		public static bool TryGetAttribute(this ISymbol symbol, string attributeName, out AttributeData? attributeData) {
			foreach (var attribute in symbol.GetAttributes()) {
				var className = attribute.AttributeClass?.GetFullName();
				if (!string.IsNullOrEmpty(className)) {
					if (className == attributeName) {
						attributeData = attribute;
						return true;
					}
				}
			}
			attributeData = null;
			return false;
		}

		public static bool HasAttributeWithBaseType(this ISymbol symbol, string baseTypeName) {
			foreach (var attribute in symbol.GetAttributes()) {
				var className = attribute.AttributeClass?.BaseType?.GetFullName();
				if (!string.IsNullOrEmpty(className)) {
					if (className == baseTypeName) {
						return true;
					}
				}
			}
			return false;
		}

		public static bool HasAttributeWithArguments(this ISymbol symbol, string attributeName, params string[] parameter) {
			foreach (var attribute in symbol.GetAttributes()) {
				var className = attribute.AttributeClass?.GetFullName();
				if (!string.IsNullOrEmpty(className)) {
					if (className == attributeName) {
						var match = attribute.ConstructorArguments.Select(x => (x.Value as INamedTypeSymbol)?.GetFullName()).SequenceEqual(parameter);
						if (match) {
							return true;
						}
					}
				}
			}
			return false;
		}

		public static bool TryGetNamedArgument(this AttributeData attributeData, string name, out TypedConstant result) {
			var argument = attributeData.NamedArguments.Where(x => x.Key == name).Select<KeyValuePair<string, TypedConstant>, TypedConstant?>(x => x.Value).FirstOrDefault();
			if (argument != null) {
				result = argument.Value;
				return true;
			} else {
				result = new TypedConstant();
				return false;
			}
		}
	}
}
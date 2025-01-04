using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.CodeGen.TypeScript {
	public class DefaultTypeScriptSourceLookup : ISourceLookup {
		private readonly Dictionary<string, string> source;

		public DefaultTypeScriptSourceLookup(Dictionary<string, string> source) {
			this.source = source;
		}

		public bool TryGet(ITypeSymbol symbol, [NotNullWhen(true)] out ISourceExpression? module) {
			if (symbol is INamedTypeSymbol named) {
				var fullName = named.GetFullName();
				foreach (var key in source.Keys) {
					if (fullName.StartsWith(key, StringComparison.InvariantCultureIgnoreCase)) {
						var value = source[key];
						module = new GenericSourceExpression(value);
						return true;
					}
				}
			}
			module = null;
			return false;
		}
	}
}
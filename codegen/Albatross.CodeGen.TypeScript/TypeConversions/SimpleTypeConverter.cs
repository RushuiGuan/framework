using Albatross.CodeAnalysis.Symbols;
using Albatross.CodeGen.Syntax;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public abstract class SimpleTypeConverter : ITypeConverter {
		public virtual int Precedence => 0;

		protected abstract IEnumerable<string> NamesToMatch { get; }
		protected abstract ITypeExpression GetResult(ITypeSymbol symbol);

		protected virtual bool IsMatch(ITypeSymbol symbol) {
			return NamesToMatch.Contains(symbol.GetFullName());
		}

		public bool TryConvert(ITypeSymbol symbol, IConvertObject<ITypeSymbol, ITypeExpression> factory, [NotNullWhen(true)] out ITypeExpression? expression) {
			if (IsMatch(symbol)) {
				expression = GetResult(symbol);
				return true;
			} else {
				expression = null;
				return false;
			}
		}
	}
}
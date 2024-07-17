using Albatross.CodeAnalysis;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertType : IConvertObject<ITypeSymbol, ITypeExpression> {
		private readonly IEnumerable<ITypeConverter> converters;

		public ConvertType(IEnumerable<ITypeConverter> converters) {
			this.converters = converters.OrderBy(x => x.Precedence).ThenBy(x => x.GetType().Name).ToList();
		}

		public ITypeExpression Convert(ITypeSymbol symbol) {
			foreach (ITypeConverter c in converters) {
				if(c.TryConvert(symbol, this, out ITypeExpression? expression)) {
					return expression;
				}	
			}
			throw new InvalidOperationException($"TypeConverter is not found for {symbol.GetFullName()}");
		}
		object IConvertObject<ITypeSymbol>.Convert(ITypeSymbol from) => this.Convert(from);
	}
}

using Albatross.CodeAnalysis;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.TypeConversions;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertType : IConvertObject<ITypeSymbol, ITypeExpression> {
		private readonly IEnumerable<ITypeConverter> converters;
		private readonly ILogger<ConvertType> logger;

		public ConvertType(IEnumerable<ITypeConverter> converters, ILogger<ConvertType> logger) {
			this.converters = converters.OrderBy(x => x.Precedence).ThenBy(x => x.GetType().Name).ToList();
			this.logger = logger;
		}

		public ITypeExpression Convert(ITypeSymbol symbol) {
			foreach (ITypeConverter c in converters) {
				if(c.TryConvert(symbol, this, out ITypeExpression? expression)) {
					logger.LogDebug("{converter} is used to convert type symbol {symbol}", c.GetType().FullName, symbol.GetFullName());
					return expression;
				}	
			}
			throw new InvalidOperationException($"TypeConverter is not found for {symbol.GetFullName()}");
		}
		object IConvertObject<ITypeSymbol>.Convert(ITypeSymbol from) => this.Convert(from);
	}
}

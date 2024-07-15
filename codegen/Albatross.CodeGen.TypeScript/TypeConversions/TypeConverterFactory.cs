using Albatross.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public interface ITypeConverterFactory {
		bool TryGet(ITypeSymbol symbol, [NotNullWhen(true)] out ITypeConverter? converter);
	}
	public class TypeConverterFactory : ITypeConverterFactory {
		private readonly IEnumerable<ITypeConverter> converters;
		private readonly ILogger<TypeConverterFactory> logger;

		public TypeConverterFactory(IEnumerable<ITypeConverter> converters, ILogger<TypeConverterFactory> logger) {
			this.converters = converters.OrderBy(x=>x.Precedence).ThenBy(x=>x.GetType().Name).ToList();
			this.logger = logger;
		}

		public bool TryGet(ITypeSymbol symbol, [NotNullWhen(true)] out ITypeConverter? converter) {
			foreach(ITypeConverter c in converters) {
				if (c.Match(symbol)) {
					converter = c;
					logger.LogDebug("Type converter {type} is being used to convert {symbol}", c.GetType().Name, symbol.GetFullName());
					return true;
				}
			}
			converter = null;
			return false;
		}
	}
}

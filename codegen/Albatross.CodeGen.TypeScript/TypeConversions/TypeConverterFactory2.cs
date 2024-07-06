using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class TypeConverterFactory2 {
		private readonly IEnumerable<ITypeConverter2> converters;

		public TypeConverterFactory2(IEnumerable<ITypeConverter2> converters) {
			this.converters = converters.OrderBy(x=>x.Precedence).ThenBy(x=>x.GetType().Name).ToList();
		}

		public bool TryGet(ITypeSymbol symbol, [NotNullWhen(true)] out ITypeConverter2? converter) {
			foreach(ITypeConverter2 c in converters) {
				if (c.Match(symbol)) {
					converter = c;
					return true;
				}
			}
			converter = null;
			return false;
		}
	}
}

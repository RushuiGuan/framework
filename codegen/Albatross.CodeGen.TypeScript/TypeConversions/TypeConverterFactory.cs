using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public interface ITypeConverterFactory {
		bool TryGet(ITypeSymbol symbol, [NotNullWhen(true)] out ITypeConverter? converter);
	}
	public class TypeConverterFactory : ITypeConverterFactory {
		private readonly IEnumerable<ITypeConverter> converters;

		public TypeConverterFactory(IEnumerable<ITypeConverter> converters) {
			this.converters = converters.OrderBy(x=>x.Precedence).ThenBy(x=>x.GetType().Name).ToList();
		}

		public bool TryGet(ITypeSymbol symbol, [NotNullWhen(true)] out ITypeConverter? converter) {
			foreach(ITypeConverter c in converters) {
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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class TypeConverterFactory {
		private readonly IEnumerable<ITypeConverter> converters;

		public TypeConverterFactory(IEnumerable<ITypeConverter> converters) {
			this.converters = converters.OrderBy(x=>x.Precedence).ThenBy(x=>x.GetType().Name).ToList();
		}

		public bool TryGet<T>([NotNullWhen(true)] out ITypeConverter? converter) => TryGet(typeof(T), out converter);
		public bool TryGet(Type type, [NotNullWhen(true)] out ITypeConverter? converter) {
			foreach(ITypeConverter c in converters) {
				if (c.Match(type)) {
					converter = c;
					return true;
				}
			}
			converter = null;
			return false;
		}
	}
}

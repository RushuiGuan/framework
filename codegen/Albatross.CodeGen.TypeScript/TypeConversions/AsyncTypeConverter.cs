using Albatross.CodeGen.TypeScript.Models;
using Albatross.Reflection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.CodeGen.TypeScript.TypeConversions {
	public class AsyncTypeConverter : ITypeConverter {
		public int Precedence => 90;
		public bool Match(Type type) => type.IsDerived<Task>();
		public TypeScriptType Convert(Type type, TypeConverterFactory factory) {
			var result = new TypeScriptType(string.Empty) {
				Name = TypeScriptType.PromiseType,
				GenericTypeArguments = type.GetGenericArguments().Select(x => factory.Convert(x)).ToArray(),
			};
			result.IsGeneric = true;
			if(!result.GenericTypeArguments.Any()) {
				result.GenericTypeArguments = [TypeScriptType.Void()];
			}
			return result;
		}
	}
}

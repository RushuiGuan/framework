using Albatross.CodeGen.TypeScript.Models;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript.Conversions {
	public class ConvertMethodInfoToMethod : IConvertObject<MethodInfo, Method> {
		ConvertParameterInfoToParameter convertToParameter;
		private readonly ConvertTypeToTypeScriptType typeConverter;

		public ConvertMethodInfoToMethod(ConvertParameterInfoToParameter convertToParameter, ConvertTypeToTypeScriptType typeConverter) {
			this.convertToParameter = convertToParameter;
			this.typeConverter = typeConverter;
		}

		public Method Convert(MethodInfo info) {
			Method method = new Method(info.Name) {
				Parameters = (from item in info.GetParameters() select convertToParameter.Convert(item)).ToArray(),
				ReturnType = typeConverter.Convert(info.ReturnType),
			};
			return method;
		}

		object IConvertObject<MethodInfo>.Convert(MethodInfo from) => this.Convert(from);
	}
}

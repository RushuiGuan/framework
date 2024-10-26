using Albatross.CodeGen.CSharp.Models;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.CSharp.Conversions {
	public class ConvertMethodInfoToMethod : IConvertObject<MethodInfo, Method> {
		ConvertParameterInfoToParameter convertToParameter;

		public ConvertMethodInfoToMethod(ConvertParameterInfoToParameter convertToParameter) {
			this.convertToParameter = convertToParameter;
		}

		public Method Convert(MethodInfo info) {
			Method method = new Method(info.Name) {
				Parameters = (from item in info.GetParameters() select convertToParameter.Convert(item)).ToArray(),
				ReturnType = new DotNetType(info.ReturnType, info.ReturnParameter),
				Static = info.IsStatic,
				Virtual = info.IsVirtual,
				AccessModifier = info.GetAccessModifier(),
			};
			return method;
		}

		object IConvertObject<MethodInfo>.Convert(MethodInfo from) => this.Convert(from);
	}
}
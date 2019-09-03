using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.CSharp.Conversion {
	public class ConvertMethodInfoToMethod: IConvertObject<MethodInfo, Method> {
        ConvertParameterInfoToParameter  convertToParameter;

		public ConvertMethodInfoToMethod(ConvertParameterInfoToParameter  convertToParameter) {
			this.convertToParameter = convertToParameter;
		}

        public Method Convert(MethodInfo info)
        {
            Method method = new Method
            {
                Name = info.Name,
                Parameters = from item in info.GetParameters() select convertToParameter.Convert(item),
                ReturnType = new DotNetType(info.ReturnType),
                Static = info.IsStatic,
                Virtual = info.IsVirtual,
                AccessModifier = info.GetAccessModifier(),
            };
            return method;
        }

        object IConvertObject<MethodInfo>.Convert(MethodInfo from)
        {
            return this.Convert(from);
        }
    }
}

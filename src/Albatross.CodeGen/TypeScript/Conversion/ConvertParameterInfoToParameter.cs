using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using Albatross.Reflection;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript.Conversion {
	public class ConvertParameterInfoToParameter : IConvertObject<ParameterInfo, Parameter> {

		public ConvertParameterInfoToParameter () {
		}

		public Parameter Convert(ParameterInfo info) {
			var p = new Parameter {
				Name = info.Name,
				Type = new TypeScriptType(info.ParameterType),
				Optional = info.ParameterType.IsNullable(),
			};
            return p;
		}

        object IConvertObject<ParameterInfo>.Convert(ParameterInfo from)
        {
            return this.Convert(from);
        }
    }
}

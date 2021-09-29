using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using Albatross.Reflection;
using System;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript.Conversion {
	public class ConvertParameterInfoToParameter : IConvertObject<ParameterInfo, Parameter> {

		public ConvertParameterInfoToParameter () {
		}

		public Parameter Convert(ParameterInfo info) {
			var p = new Parameter(info.Name?? throw new Exception("impossible"), new TypeScriptType(info.ParameterType) ){
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

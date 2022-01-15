using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System;
using System.Reflection;

namespace Albatross.CodeGen.CSharp.Conversion {
	public class ConvertParameterInfoToParameter : IConvertObject<ParameterInfo, Parameter> {
		public Parameter Convert(ParameterInfo info) {
			var p = new Parameter(info.Name ?? throw new Exception(), new DotNetType(info.ParameterType)) {
				Modifier = info.IsOut ? Model.ParameterModifier.Out : info.IsIn ? Model.ParameterModifier.In : Model.ParameterModifier.Ref,
			};

			if (info.IsOut) {
				p.Modifier = Model.ParameterModifier.Out;
			} else if (info.IsIn) {
				p.Modifier = Model.ParameterModifier.In;
			} else if (info.ParameterType.IsByRef) {
				p.Modifier = Model.ParameterModifier.Ref;
			} else {
				p.Modifier = Model.ParameterModifier.None;
			}
			return p;
		}

		object IConvertObject<ParameterInfo>.Convert(ParameterInfo from) => this.Convert(from);
	}
}

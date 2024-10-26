using Albatross.CodeGen.CSharp.Models;
using System;
using System.Reflection;

namespace Albatross.CodeGen.CSharp.Conversions {
	public class ConvertParameterInfoToParameter : IConvertObject<ParameterInfo, Parameter> {
		public Parameter Convert(ParameterInfo info) {
			var p = new Parameter(info.Name ?? throw new Exception(), new DotNetType(info.ParameterType, info)) {
				Modifier = info.IsOut ? Models.ParameterModifier.Out : info.IsIn ? Models.ParameterModifier.In : Models.ParameterModifier.Ref,
			};

			if (info.IsOut) {
				p.Modifier = Models.ParameterModifier.Out;
			} else if (info.IsIn) {
				p.Modifier = Models.ParameterModifier.In;
			} else if (info.ParameterType.IsByRef) {
				p.Modifier = Models.ParameterModifier.Ref;
			} else {
				p.Modifier = Models.ParameterModifier.None;
			}
			return p;
		}

		object IConvertObject<ParameterInfo>.Convert(ParameterInfo from) => this.Convert(from);
	}
}
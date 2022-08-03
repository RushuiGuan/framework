﻿using Albatross.CodeGen.Core;
using Albatross.CodeGen.TypeScript.Model;
using Albatross.Reflection;
using System;
using System.Reflection;

namespace Albatross.CodeGen.TypeScript.Conversion {
	public class ConvertParameterInfoToParameter : IConvertObject<ParameterInfo, ParameterDeclaration> {
		private readonly ConvertTypeToTypeScriptType typeConverter;

		public ConvertParameterInfoToParameter(ConvertTypeToTypeScriptType typeConverter) {
			this.typeConverter = typeConverter;
		}

		public ParameterDeclaration Convert(ParameterInfo info) {
			var p = new ParameterDeclaration(info.Name ?? throw new Exception("impossible"), 
				typeConverter.Convert(info.ParameterType), AccessModifier.None) {
				Optional = info.ParameterType.IsNullable(),
			};
			return p;
		}

		object IConvertObject<ParameterInfo>.Convert(ParameterInfo from) => this.Convert(from);
	}
}

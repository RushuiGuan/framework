using Albatross.CodeGen.Python.Models;
using System;
using System.Reflection;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertFieldInfoToField : IConvertObject<FieldInfo, Field> {
		private readonly IConvertObject<Type, PythonType> typeConverter;

		public ConvertFieldInfoToField(IConvertObject<Type, PythonType> typeConverter) {
			this.typeConverter = typeConverter;
		}

		public Field Convert(FieldInfo from) {
			var pythonType = typeConverter.Convert(from.FieldType);
			var name = Extensions.GetPythonFieldName(from.Name);
			return new Field(name, pythonType, pythonType.DefaultValue);
		}

		object IConvertObject<FieldInfo>.Convert(FieldInfo from) {
			return this.Convert(from);
		}
	}
}
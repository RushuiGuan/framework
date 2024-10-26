using Albatross.CodeGen.Python.Models;
using System;
using System.Reflection;
using System.Text;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertPropertyInfoToField : IConvertObject<PropertyInfo, Field> {
		private readonly IConvertObject<Type, PythonType> typeConverter;

		public ConvertPropertyInfoToField(IConvertObject<Type, PythonType> typeConverter) {
			this.typeConverter = typeConverter;
		}

		public Field Convert(PropertyInfo from) {
			var pythonType = typeConverter.Convert(from.PropertyType);
			var name = Extensions.GetPythonFieldName(from.Name);
			return new Field(name, pythonType, pythonType.DefaultValue);
		}

		object IConvertObject<PropertyInfo>.Convert(PropertyInfo from) {
			return this.Convert(from);
		}

	}
}
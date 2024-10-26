using Albatross.CodeGen.Python.Models;
using System;
using System.Reflection;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertPropertyInfoToProperty : IConvertObject<PropertyInfo, Property> {
		private readonly IConvertObject<Type, PythonType> typeConverter;

		public ConvertPropertyInfoToProperty(IConvertObject<Type, PythonType> typeConverter) {
			this.typeConverter = typeConverter;
		}

		public Property Convert(PropertyInfo from) {
			var pythonType = typeConverter.Convert(from.PropertyType);
			return new Property(from.Name, pythonType, new NoneValue());
		}

		object IConvertObject<PropertyInfo>.Convert(PropertyInfo from) {
			return this.Convert(from);
		}
	}
}
using Albatross.CodeGen.Python.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertTypeToPythonClass : IConvertObject<Type, Class> {
		IConvertObject<PropertyInfo, Property> convertProperty;
		IConvertObject<FieldInfo, Field> convertField;

		public ConvertTypeToPythonClass(IConvertObject<PropertyInfo, Property> convertProperty, IConvertObject<FieldInfo, Field> convertField) {
			this.convertProperty = convertProperty;
			this.convertField = convertField;
		}

		public Class Convert(Type type) {
			var result = new Class(type.Name);
			if (type.BaseType != null && type.BaseType != typeof(object)) {
				result.BaseClass = [Convert(type.BaseType)];
			}

			result.Properties = (from p in type.GetProperties() select convertProperty.Convert(p)).ToList();
			result.Fields = (from f in type.GetFields() select convertField.Convert(f)).ToList();
			return result;
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
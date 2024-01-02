using Albatross.CodeGen.Python.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertTypeToPythonClass : IConvertObject<Type, Class> {
		IConvertObject<PropertyInfo, Field> convertProperty;
		IConvertObject<FieldInfo, Field> convertField;

		public ConvertTypeToPythonClass(IConvertObject<PropertyInfo, Field> convertProperty, IConvertObject<FieldInfo, Field> convertField) {
			this.convertProperty = convertProperty;
			this.convertField = convertField;
		}

		public Class Convert(Type type) {
			var result = new Class(type.Name);
			if (type.BaseType != null && type.BaseType != typeof(object)) {
				result.BaseClass = [Convert(type.BaseType)];
			}

			result.Fields = (from f in type.GetFields() select convertField.Convert(f)).ToList();
			result.Fields.AddRange(from p in type.GetProperties() select convertProperty.Convert(p));
			return result;
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
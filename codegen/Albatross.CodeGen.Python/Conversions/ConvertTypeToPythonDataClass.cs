using Albatross.CodeGen.Python.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertTypeToPythonDataClass : IConvertObject<Type, Class> {
		IConvertObject<PropertyInfo, Field> convertProperty;
		IConvertObject<FieldInfo, Field> convertField;

		public ConvertTypeToPythonDataClass(IConvertObject<PropertyInfo, Field> convertProperty, IConvertObject<FieldInfo, Field> convertField) {
			this.convertProperty = convertProperty;
			this.convertField = convertField;
		}

		public Class Convert(Type type) {
			var result = new Class(type.Name);
			if (type.BaseType != null && type.BaseType != typeof(object)) {
				result.AddBaseClass(Convert(type.BaseType));
			}
			foreach(var field in type.GetFields().Where(x=>!x.IsStatic)) {
				var item = convertField.Convert(field);
				item.Static = true;
				result.AddField(item);
			}
			foreach(var property in type.GetProperties()) {
				var item = convertProperty.Convert(property);
				item.Static = true;
				result.AddField(item);
			}
			result.AddDecorator(My.Decorators.DataClass());
			return result;
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
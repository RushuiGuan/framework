using Albatross.CodeGen.Python.Models;
using System;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertTypeToPythonClass : IConvertObject<Type, ClassDeclaration> {
		IConvertObject<PropertyInfo, Field> convertProperty;
		IConvertObject<FieldInfo, Field> convertField;

		public ConvertTypeToPythonClass(IConvertObject<PropertyInfo, Field> convertProperty, IConvertObject<FieldInfo, Field> convertField) {
			this.convertProperty = convertProperty;
			this.convertField = convertField;
		}

		public ClassDeclaration Convert(Type type) {
			var result = new ClassDeclaration(type.Name);
			if (type.BaseType != null && type.BaseType != typeof(object)) {
				result.AddBaseClass(Convert(type.BaseType));
			}
			foreach (var field in type.GetFields().Where(x => !x.IsStatic)) {
				var item = convertField.Convert(field);
				result.AddField(item);
			}
			foreach (var property in type.GetProperties()) {
				var item = convertProperty.Convert(property);
				result.AddField(item);
			}
			return result;
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}
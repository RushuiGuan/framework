using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.CSharp.Conversion {
	public class ConvertTypeToCSharpClass : IConvertObject<Type, Class> {
        IConvertObject<PropertyInfo, Property> convertProperty;
        IConvertObject<FieldInfo, Field> convertField;
        public ConvertTypeToCSharpClass(IConvertObject<PropertyInfo, Property> convertProperty, IConvertObject<FieldInfo, Field> convertField) {
            this.convertProperty = convertProperty;
            this.convertField = convertField;
        }
        public Class Convert(Type type)
        {
			var result = new Class {
				Namespace = type.Namespace,
				AccessModifier = type.IsPublic ? AccessModifier.Public : AccessModifier.None,
				Sealed = type.IsSealed,
				Static = type.IsAbstract && type.IsSealed,
				Abstract = type.IsAbstract && !type.IsSealed,
				Name = type.Name,
			};

            if (type.BaseType != null &&  type.BaseType != typeof(object))
            {
                result.BaseClass = Convert(type.BaseType);
            }


			result.Properties = (from p in type.GetProperties() select convertProperty.Convert(p)).ToArray();
			result.Fields = (from f in type.GetFields() select convertField.Convert(f)).ToArray();
            return result;
        }

        object IConvertObject<Type>.Convert(Type from)
        {
            return this.Convert(from);
        }
    }
}

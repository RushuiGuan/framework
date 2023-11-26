using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System;
using System.Linq;
using System.Reflection;

namespace Albatross.CodeGen.CSharp.Conversion {
	public class ConvertPropertyInfoToProperty : IConvertObject<PropertyInfo, Property> {
		public Property Convert(PropertyInfo from) {
			Property property = new Property(from.Name, new DotNetType(from.PropertyType)) {
				CanWrite = from.CanWrite,
				CanRead = from.CanRead,
				Static = from.GetAccessors().Any(args => args.IsStatic),
				Modifier = from.GetMethod?.GetAccessModifier() ?? AccessModifier.Private,
			};
			property.SetModifier = (from.GetSetMethod() ?? from.GetSetMethod(true))?.GetAccessModifier() ?? property.Modifier;
			return property;
		}

		object IConvertObject<PropertyInfo>.Convert(PropertyInfo from) => this.Convert(from);
	}
}

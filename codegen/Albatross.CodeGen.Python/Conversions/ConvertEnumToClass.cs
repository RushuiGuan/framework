using Albatross.CodeGen.Python.Models;
using System;
using System.Linq;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertEnumToClass : IConvertObject<Type, Class> {
		public Class Convert(Type type) {
			if (type.IsEnum) {
				var model = new Class(type.Name);
				model.AddCodeElement<Class>(My.Classes.Enum(), nameof(Class.BaseClass));
				foreach(var value in Enum.GetValues(type)) {
					var name = Enum.GetName(type, value)?.ToUpper() ?? throw new Exception();
					var field = new Field(name, My.Types.NoType(), new Literal((int)value)) {
						Static = true,
					};
					model.AddCodeElement(field, nameof(Class.Fields));
				}
				return model;
			} else {
				throw new InvalidOperationException($"Type {type.Name} is not an Enum type");
			}
		}
		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}

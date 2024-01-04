using Albatross.CodeGen.Python.Models;
using System;
using System.Linq;

namespace Albatross.CodeGen.Python.Conversions {
	public class ConvertEnumToClass : IConvertObject<Type, Class> {
		public Class Convert(Type type) {
			if (type.IsEnum) {
				var model = new Class(type.Name) {
					BaseClass = [My.Classes.Enum],
					Fields = Enum.GetValues(type)
						.Cast<object>()
						.Select(x => new Field(Enum.GetName(type, x)?.ToUpper() ?? throw new Exception(), My.Types.NoType, new Literal((int)x)) {
							Static = true,
						})
						.ToList(),
				};
				return model;
			} else {
				throw new InvalidOperationException($"Type {type.Name} is not an Enum type");
			}
		}
		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}

using Albatross.CodeGen.CSharp.Models;
using System.Reflection;

namespace Albatross.CodeGen.CSharp.Conversions {
	public class ConvertFieldInfoToField : IConvertObject<FieldInfo, Field> {
		public Field Convert(FieldInfo from) {
			return new Field(from.Name, new DotNetType(from.FieldType)) {
				ReadOnly = from.IsInitOnly,
			};
		}

		object IConvertObject<FieldInfo>.Convert(FieldInfo from) {
			return this.Convert(from);
		}
	}
}
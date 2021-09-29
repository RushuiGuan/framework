using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using System.Reflection;

namespace Albatross.CodeGen.CSharp.Conversion {
	public class ConvertFieldInfoToField : IConvertObject<FieldInfo, Field> {
        public ConvertFieldInfoToField() {
        }

        public Field Convert(FieldInfo from)
        {
            return new Field(from.Name, new DotNetType(from.FieldType))
            {
                ReadOnly = from.IsInitOnly,
            };
        }

        object IConvertObject<FieldInfo>.Convert(FieldInfo from)
        {
            return this.Convert(from);
        }
    }
}

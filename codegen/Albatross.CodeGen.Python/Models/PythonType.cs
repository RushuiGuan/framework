using Albatross.Text;
using System.IO;
using System.Linq;

namespace Albatross.CodeGen.Python.Models {
	public record class PythonType : ICodeElement{
		public string Name { get; set; }
		public PythonType(string name) {
			Name = name;
		}

		public TextWriter Generate(TextWriter writer) {
			writer.Append(Name);
			return writer;
		}
		
		public override string? ToString() {
			StringWriter writer = new StringWriter();
			return writer.Code(this).ToString();
		}
		
		public static PythonType NoneType() => new PythonType("None");
		public static PythonType String() => new PythonType("str");
		public static PythonType Char() => new PythonType("str");

		public static PythonType Int() => new PythonType("int");
		public static PythonType Decimal() => new PythonType("decimal");
		public static PythonType Float() => new PythonType("float");

		public static PythonType DateTime() => new PythonType("datetime");
		public static PythonType Date() => new PythonType("date");
		public static PythonType Time() => new PythonType("time");
		public static PythonType TimeDelta() => new PythonType("timedelta");

		public static PythonType Boolean() => new PythonType("bool");
		public static PythonType Dictionary() => new PythonType("dict");
		public static PythonType List() => new PythonType("list");
		public static PythonType Tuple() => new PythonType("tuple");
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Python.Models {
	public static class My {
		public static class Decorators {
			public readonly static Decorator StaticMethod = new Decorator("staticmethod");
		}

		public static class Classes {
			public readonly static Class Enum = new Class("Enum");
			public readonly static Class AbstractBaseClass = new Class("ABC");
		}

		public static class Keywords {
			public const string Self = "self";
			public const string Def = "def";
		}

		public static class Types {
			public readonly static PythonType NoneType = new PythonType("None");
			public readonly static PythonType String = new PythonType("str") { DefaultValue = new StringLiteral(string.Empty) };
			public readonly static PythonType Char = new PythonType("str") { DefaultValue = new StringLiteral(string.Empty) };

			public readonly static PythonType Int = new PythonType("int") { DefaultValue = new Literal(0) };
			public readonly static PythonType Decimal = new PythonType("decimal") { DefaultValue = new Literal(0) };
			public readonly static PythonType Float = new PythonType("float") { DefaultValue = new Literal(0) };

			public readonly static PythonType DateTime = new PythonType("datetime") { DefaultValue = new Literal("datetime.datetime.min") };
            public readonly static PythonType Date = new PythonType("date") { DefaultValue = new Literal("datetime.date.min") };
            public readonly static PythonType Time = new PythonType("time") { DefaultValue = new Literal("datetime.time.min") };
            public readonly static PythonType TimeDelta = new PythonType("timedelta") { DefaultValue = new Literal("datetime.timedelta.min") };

            public readonly static PythonType Boolean = new PythonType("bool") { DefaultValue = new Literal(false) };
			public readonly static PythonType Dictionary = new PythonType("dict");
			public readonly static PythonType List = new PythonType("list") { DefaultValue = new Literal("[]") };
			public readonly static PythonType Tuple = new PythonType("tuple");
		}
	}
}

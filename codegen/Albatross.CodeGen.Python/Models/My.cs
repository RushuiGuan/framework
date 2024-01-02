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
			public readonly static PythonType String = new PythonType("str");
			public readonly static PythonType Char = new PythonType("str");

			public readonly static PythonType Int = new PythonType("int");
			public readonly static PythonType Decimal = new PythonType("decimal");
			public readonly static PythonType Float = new PythonType("float");

			public readonly static PythonType DateTime = new PythonType("datetime");
			public readonly static PythonType Date = new PythonType("date");
			public readonly static PythonType Time = new PythonType("time");
			public readonly static PythonType TimeDelta = new PythonType("timedelta");

			public readonly static PythonType Boolean = new PythonType("bool");
			public readonly static PythonType Dictionary = new PythonType("dict");
			public readonly static PythonType List = new PythonType("list");
			public readonly static PythonType Tuple = new PythonType("tuple");
		}
	}
}

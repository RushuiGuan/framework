namespace Albatross.CodeGen.Python.Models {
	public static class My {
		public static class Decorators {
			public readonly static Decorator StaticMethod = new Decorator("staticmethod");
			public readonly static Decorator AbstractMethod = new Decorator("abstractmethod") { Module = "abc" };
			public readonly static Decorator DataClass = new Decorator("dataclass") { Module = "dataclasses" };
		}

		public static class Classes {
			public readonly static Class Enum = new Class("Enum") { Module = "enum" };
			public readonly static Class AbstractBaseClass = new Class("ABC") { Module = "abc" };
		}

		public static class Keywords {
			public const string Self = "self";
			public const string Def = "def";
		}

		public static class Types {
			public readonly static PythonType NoneType = new PythonType("None");
			public readonly static PythonType NoType = new PythonType("");
			public readonly static PythonType AnyType = new PythonType("Any") {
				Module = "typing",
			};
			public readonly static PythonType String = new PythonType("str") { DefaultValue = new StringLiteral(string.Empty) };
			public readonly static PythonType Char = new PythonType("str") { DefaultValue = new StringLiteral(string.Empty) };

			public readonly static PythonType Int = new PythonType("int") { DefaultValue = new Literal(0) };
			public readonly static PythonType Decimal = new PythonType("Decimal") {
				DefaultValue = new Literal(0),
				Module = "decimal"
			};
			public readonly static PythonType Float = new PythonType("float") { DefaultValue = new Literal(0) };

			public readonly static PythonType DateTime = new PythonType("datetime") {
				DefaultValue = new Literal("datetime.min"),
				Module = "datetime"
			};
			public readonly static PythonType Date = new PythonType("date") {
				DefaultValue = new Literal("date.min"),
				Module = "datetime"
			};
			public readonly static PythonType Time = new PythonType("time") {
				DefaultValue = new Literal("time.min"),
				Module = "datetime"
			};
			public readonly static PythonType TimeDelta = new PythonType("timedelta") {
				DefaultValue = new Literal("timedelta.min"),
				Module = "datetime"
			};

			public readonly static PythonType Boolean = new PythonType("bool") { DefaultValue = new Literal(false) };
			public readonly static PythonType Dictionary = new PythonType("dict") {
				DefaultValue = new MethodCall("field") {
					Module = "dataclasses",
					Parameters = [new AssignmentCodeBlock("default_factory", new Literal("dict"))]
				},
			};
			public readonly static PythonType List = new PythonType("list") {
				DefaultValue = new MethodCall("field") {
					Module = "dataclasses",
					Parameters = [new AssignmentCodeBlock("default_factory", new Literal("list"))]
				},
			};
			public readonly static PythonType Tuple = new PythonType("tuple");
		}
	}
}

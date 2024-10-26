namespace Albatross.CodeGen.Python.Models {
	public static class My {
		public static class Keywords {
			public const string Self = "self";
			public const string Def = "def";
		}

		public static class Modules {
			public const string ABC = "abc";
			public const string DataClasses = "dataclasses";
			public const string Enum = "enum";
			public const string Typing = "typing";
			public const string Decimal = "decimal";
			public const string DateTime = "datetime";
			public const string Requests = "requests";
			public const string Pandas = "pandas";
			public const string Json = "json";
		}
		public static class Classes {
			public const string EnumName = "Enum";
			public static ClassDeclaration Enum() => new ClassDeclaration(EnumName, Modules.Enum);
			public static ClassDeclaration AbstractBaseClass() => new ClassDeclaration("ABC", Modules.ABC);
		}

		public static class Methods {
			public static Method ListMethod() => new Method("list");
			public static Method DictMethod() => new Method("dict");
			public static Method TupleMethod() => new Method("tuple");
			public static Method Field() => new Method("field", Modules.DataClasses);
		}

		public static class Decorators {
			public const string StaticMethodName = "staticmethod";
			public const string DataClassName = "dataclass";
			public const string AbstractMethodName = "abstractmethod";
			public static Decorator StaticMethod() => new Decorator(StaticMethodName);
			public static Decorator AbstractMethod() => new Decorator(AbstractMethodName, Modules.ABC);
			public static Decorator DataClass() => new Decorator(DataClassName, Modules.DataClasses);
		}
		public static class Types {
			public static PythonType NoneType() => new PythonType("None");
			public static PythonType NoType() => new PythonType(string.Empty);
			public static PythonType String() => new PythonType("str") { DefaultValue = new StringLiteral(string.Empty) };
			public static PythonType Int() => new PythonType("int") { DefaultValue = new Literal(0) };
			public static PythonType Float() => new PythonType("float") { DefaultValue = new Literal(0) };

			public static PythonType AnyType() => new PythonType("Any", Modules.Typing);
			public static PythonType Decimal() => new PythonType("Decimal", Modules.Decimal) { DefaultValue = new Literal(0), };
			public static PythonType DateTime() => new PythonType("datetime", Modules.DateTime) {
				DefaultValue = new Literal("datetime.min"),
			};
			public static PythonType Date() => new PythonType("date", Modules.DateTime) {
				DefaultValue = new Literal("date.min"),
			};
			public static PythonType Time() => new PythonType("time", Modules.DateTime) {
				DefaultValue = new Literal("time.min"),
			};
			public static PythonType TimeDelta() => new PythonType("timedelta", Modules.DateTime) {
				DefaultValue = new Literal("timedelta.min"),
			};
			public static PythonType Boolean() => new PythonType("bool") {
				DefaultValue = new Literal(false)
			};
			public static PythonType Dictionary() => new PythonType("dict") {
				DefaultValue = new MethodCall(My.Methods.Field(), new Assignment("default_factory", new Literal("dict")))
			};
			public static PythonType List() => new PythonType("list") {
				DefaultValue = new MethodCall(Methods.Field(), new Assignment("default_factory", new Literal("list")))
			};
			public static PythonType Tuple() => new PythonType("tuple") {
				DefaultValue = new MethodCall(Methods.Field(), new Assignment("default_factory", new Literal("tuple")))
			};

			public static PythonType DataFrame() => new PythonType("DataFrame") {
				Module = "pandas"
			};
		}
	}
}
using Albatross.CodeGen.TypeScript.Expressions;

namespace Albatross.CodeGen.WebClient {
	public static class My {
		public static class Sources {
			public readonly static ISourceExpression MirageLogging = new ModuleSourceExpression("@mirage/logging");
			public readonly static ISourceExpression MirageWebClient = new ModuleSourceExpression("@mirage/webclient");
			public readonly static ISourceExpression MirageConfig = new ModuleSourceExpression("@mirage/config");
		}
		public static class  Identifiers {
			public readonly static IIdentifierNameExpression Logger = new QualifiedIdentifierNameExpression("Logger", Sources.MirageLogging);

		}
		public static class Types {
			public readonly static ITypeExpression Logger = new SimpleTypeExpression {
				Identifier = Identifiers.Logger,
			};
		}
	}
}

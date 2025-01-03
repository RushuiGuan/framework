using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.TypeScript {
	public static class Defined {
		public static class Patterns {
			public static Regex IdentifierName => new Regex(@"^\w\w*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			public static Regex ModuleSource => new Regex(@"^(@\w+/)? [\w\-]+ (/\w+)*$", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}
		public static class Types {
			public static SimpleTypeExpression Any(bool optional = false) => new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("any"),
				Optional = optional
			};
			public static SimpleTypeExpression Void() => new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("void")
			};
			public static SimpleTypeExpression Object(bool optional = false) => new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("object"),
				Optional = optional
			};
			public static SimpleTypeExpression Boolean(bool optional = false) => new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("boolean"),
				Optional = optional
			};

			public static SimpleTypeExpression Date(bool optional = false) => new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("Date"),
				Optional = optional
			};
			public static SimpleTypeExpression Numeric(bool optional = false) => new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("number"),
				Optional = optional
			};
			public static SimpleTypeExpression String(bool optional = false) => new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("string"),
				Optional = optional
			};
			public static SimpleTypeExpression Null() => new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("null")
			};
			public static SimpleTypeExpression Undefined() => new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("undefined")
			};
			public static SimpleTypeExpression Type(string name, bool optional) {
				return new SimpleTypeExpression {
					Identifier = new IdentifierNameExpression(name),
					Optional = optional
				};
			}
			public static SimpleTypeExpression HttpClient(bool optional = false) => new SimpleTypeExpression {
				Identifier = Identifiers.HttpClient,
				Optional = optional
			};
		}

		public static class Literals {
			public static StringLiteralExpression String(string value)
				=> new StringLiteralExpression(value);
			public static NumberLiteralExpression Number(int value)
				=> new NumberLiteralExpression(value);
			public static NumberLiteralExpression NumberLiteral(double value)
				=> new NumberLiteralExpression(value);
			public static BooleanLiteralExpression BooleanLiteral(bool value)
				=> new BooleanLiteralExpression(value);
			public static JsonPropertyExpression JsonProperty(string name, string value)
				=> new JsonPropertyExpression(name, new StringLiteralExpression(value));
		}

		public static class Invocations {
			public static InvocationExpression InjectableDecorator(string providedIn) {
				return new DecoratorExpression {
					Identifier = new QualifiedIdentifierNameExpression("Injectable", Sources.AngularCore),
					ArgumentList = new ListOfSyntaxNodes<IExpression>(new JsonValueExpression(new JsonPropertyExpression("providedIn", new StringLiteralExpression(providedIn))))
				};
			}
			public static InvocationExpression ConsoleLog(string message) {
				return new InvocationExpression {
					Identifier = new MultiPartIdentifierNameExpression(new IdentifierNameExpression("console"), new IdentifierNameExpression("log")),
					ArgumentList = new ListOfSyntaxNodes<IExpression>(new StringLiteralExpression(message)),
					Terminate = true,
				};
			}
		}

		public static class Sources {
			public static ISourceExpression AngularCore => new ModuleSourceExpression("@angular/core");
			public static ISourceExpression AngularHttp => new ModuleSourceExpression("@angular/common/http");
			public static ISourceExpression Rxjs => new ModuleSourceExpression("rxjs");
			public static ISourceExpression DateFns => new ModuleSourceExpression("date-fns");
		}

		public static class Identifiers {
			public static IIdentifierNameExpression HttpClient => new QualifiedIdentifierNameExpression("HttpClient", Sources.AngularHttp);
			public static IIdentifierNameExpression This => new IdentifierNameExpression("this");
			public static IIdentifierNameExpression Promise => new IdentifierNameExpression("Promise");
			public static IIdentifierNameExpression Observable => new QualifiedIdentifierNameExpression("Observable", Sources.Rxjs);
			public static IIdentifierNameExpression FirstValueFrom => new QualifiedIdentifierNameExpression("firstValueFrom", Sources.Rxjs);
		}
	}
}
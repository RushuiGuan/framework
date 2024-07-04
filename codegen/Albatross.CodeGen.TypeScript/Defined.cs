using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript.Expressions;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.TypeScript {
	public static class Defined {
		public static class Patterns {
			public readonly static Regex IdentifierName = new Regex(@"^[@a-zA-Z_]\w*$", RegexOptions.Compiled);
			public readonly static Regex ModuleSource = new Regex(@"^(@[a-zA-Z0-9]+/)?[a-zA-Z0-9]+$", RegexOptions.Compiled);
		}
		public static class Types {
			public readonly static SimpleTypeExpression Any = new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("any"),
			};
			public readonly static SimpleTypeExpression Void = new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("void")
			};

			public readonly static SimpleTypeExpression Boolean = new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("boolean")
			};

			public readonly static SimpleTypeExpression Date = new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("Date")
			};
			public readonly static SimpleTypeExpression Numeric = new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("number")
			};
			public readonly static SimpleTypeExpression String = new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("string")
			};
			public readonly static SimpleTypeExpression Null = new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("null")
			};
			public readonly static SimpleTypeExpression Undefined = new SimpleTypeExpression {
				Identifier = new IdentifierNameExpression("undefined")
			};
			public static SimpleTypeExpression Type(string name) {
				return new SimpleTypeExpression {
					Identifier = new IdentifierNameExpression(name)
				};
			}
			public readonly static SimpleTypeExpression HttpClient = new SimpleTypeExpression {
				Identifier = Identifiers.HttpClient,
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
				return new InvocationExpression {
					Identifier = Identifiers.Injectable,
					ArgumentList = new ListOfSyntaxNodes<IExpression>(new ObjectLiteralExpression {
						Properties = new ListOfSyntaxNodes<JsonPropertyExpression>(new JsonPropertyExpression(providedIn, new StringLiteralExpression(providedIn)))
					})
				};
			}
		}
		
		public static class Sources {
			public readonly static ISourceExpression AngularCore = new ModuleSourceExpression("@angular/core");
			public readonly static ISourceExpression AngularHttp = new ModuleSourceExpression("@angular/common/http");
		}

		public static class Identifiers {
			public readonly static IIdentifierNameExpression Injectable = new QualifiedIdentifierNameExpression("@Injectable", Sources.AngularCore);
			public readonly static IIdentifierNameExpression HttpClient = new QualifiedIdentifierNameExpression("HttpClient", Sources.AngularHttp);
			public readonly static IIdentifierNameExpression This = new IdentifierNameExpression("this");
		}
	}
}

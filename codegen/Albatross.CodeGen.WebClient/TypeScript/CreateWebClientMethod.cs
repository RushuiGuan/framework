using Albatross.CodeAnalysis;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.Modifiers;
using Albatross.Text;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Albatross.CodeGen.WebClient.TypeScript {
	public interface ICreateWebClientMethod : IConvertObject<IMethodSymbol, MethodDeclaration> { }

	public class CreateWebClientMethod : ICreateWebClientMethod {
		private readonly TypeScriptWebClientSettings settings;
		private readonly IConvertObject<IParameterSymbol, ParameterDeclaration> parameterConverter;
		private readonly IConvertObject<ITypeSymbol, ITypeExpression> typeConverter;

		public CreateWebClientMethod(TypeScriptWebClientSettings settings, 
			IConvertObject<IParameterSymbol, ParameterDeclaration> parameterConverter, IConvertObject<ITypeSymbol, ITypeExpression> typeConverter) {
			this.settings = settings;
			this.parameterConverter = parameterConverter;
			this.typeConverter = typeConverter;
		}
		public MethodDeclaration Convert(IMethodSymbol methodSymbol) {
			return new MethodDeclaration(methodSymbol.Name.CamelCase()) {
				Modifiers = [new AsyncModifier()],
				ReturnType = typeConverter.Convert(methodSymbol.ReturnType).ToPromise(),
				Parameters = new ListOfSyntaxNodes<ParameterDeclaration>(methodSymbol.Parameters.Select(x => this.parameterConverter.Convert(x))),
				Body = new ScopedVariableExpressionBuilder()
					.WithName("relativeUrl").WithExpression(methodSymbol.GetRoute().ConvertRoute2StringInterpolation())
					.Add(()=>CreateHttpInvocationExpression(methodSymbol))
					.BuildAll()
			};
		}
		object IConvertObject<IMethodSymbol>.Convert(IMethodSymbol from) => Convert(from);

		IExpression CreateHttpInvocationExpression(IMethodSymbol methodSymbol) {
			var builder = new InvocationExpressionBuilder();
			builder.Await();
			var returnType = this.typeConverter.Convert(methodSymbol.ReturnType);
			var hasVoidReturnType = object.Equals(returnType, Defined.Types.Void());
			var hasStringReturnType = object.Equals(returnType, Defined.Types.String());
			switch (methodSymbol.GetHttpMethod()) {
				case "get":
					if (hasStringReturnType) {
						builder.WithName("doGetStringAsync");
					} else {
						builder.WithName("doGetAsync").AddGenericArgument(returnType);
					}
					break;
				case "post":
					if (hasStringReturnType) {
						builder.WithName("doPostStringAsync");
					} else {
						builder.WithName("doPostAsync");
						builder.AddGenericArgument(returnType);
					}
					break;
				case "patch":
					if (hasStringReturnType) {
						builder.WithName("doPatchStringAsync");
					} else {
						builder.WithName("doPatchAsync");
						builder.AddGenericArgument(returnType);
					}
					break;
				case "put":
					if (hasStringReturnType) {
						builder.WithName("doPutStringAsync");
					} else {
						builder.WithName("doPutAsync");
						builder.AddGenericArgument(returnType);
					}
					break;
				case "delete":
					builder.WithName("doDeleteAsync");
					break;
			}
			
			builder.AddArgument(new IdentifierNameExpression("relativeUrl"));

			var fromBodyParameter = methodSymbol.Parameters.FirstOrDefault(x => x.HasAttribute("Microsoft.AspNetCore.Mvc.FromBodyAttribute"));
			if(fromBodyParameter!= null) {
				builder.AddGenericArgument(this.typeConverter.Convert(fromBodyParameter.Type));
				builder.AddArgument(new IdentifierNameExpression(fromBodyParameter.Name.CamelCase()));
			}
			if (!hasVoidReturnType) {
				return new ScopedVariableExpressionBuilder()
					.IsConstant().WithName("result").WithExpression(builder.Build())
					.Add(() => new ReturnExpression(new IdentifierNameExpression("result")))
					.BuildAll();
			} else {
				return builder.Build();
			}
		}
	}
}

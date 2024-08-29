using Albatross.CodeAnalysis;
using Albatross.CodeGen.Syntax;
using Albatross.CodeGen.TypeScript;
using Albatross.CodeGen.TypeScript.Declarations;
using Albatross.CodeGen.TypeScript.Expressions;
using Albatross.CodeGen.TypeScript.Modifiers;
using Albatross.Text;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
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
			var returnType = typeConverter.Convert(methodSymbol.ReturnType);
			if (object.Equals(returnType, Defined.Types.Void())) {
				returnType = Defined.Types.Object();
			}
			return new MethodDeclaration(methodSymbol.Name.CamelCase()) {
				Modifiers = settings.UsePromise ? [new AsyncModifier()] : [],
				ReturnType = settings.UsePromise ? returnType.ToPromise() : returnType.ToObservable(),
				Parameters = new ListOfSyntaxNodes<ParameterDeclaration>(methodSymbol.Parameters.Select(x => this.parameterConverter.Convert(x))),
				Body = new ScopedVariableExpressionBuilder()
					.IsConstant()
					.WithName("relativeUrl").WithExpression(methodSymbol.GetRoute().ConvertRoute2StringInterpolation())
					.Add(() => CreateHttpInvocationExpression(methodSymbol))
					.BuildAll()
			};
		}
		object IConvertObject<IMethodSymbol>.Convert(IMethodSymbol from) => Convert(from);

		void ProcessMethodParameters(IMethodSymbol methodSymbol, List<IParameterSymbol> routeParameters, List<IParameterSymbol> queryParams, List<IParameterSymbol> bodyParams) {
			if (methodSymbol.TryGetAttribute("Microsoft.AspNetCore.Mvc.RouteAttribute", out var routeAttribe)) {
			}
			foreach (var parameter in methodSymbol.Parameters) {
				if (parameter.HasAttribute("Microsoft.AspNetCore.Mvc.FromRouteAttribute")) {
					routeParameters.Add(parameter);
				} else if (parameter.HasAttribute("Microsoft.AspNetCore.Mvc.FromBodyAttribute")) {
					bodyParams.Add(parameter);
				} else if (parameter.HasAttribute("Microsoft.AspNetCore.Mvc.FromQueryAttribute")) {
					queryParams.Add(parameter);
				} else {

				}
			}
		}
		IExpression CreateHttpInvocationExpression(IMethodSymbol methodSymbol) {
			var builder = new CodeGen.TypeScript.Expressions.InvocationExpressionBuilder();
			if (settings.UsePromise) {
				builder.Await();
			}
			var returnType = this.typeConverter.Convert(methodSymbol.ReturnType);
			var hasVoidReturnType = false;
			if (object.Equals(returnType, Defined.Types.Void())) {
				hasVoidReturnType = true;
				returnType = Defined.Types.Object();
			}
			var hasStringReturnType = object.Equals(returnType, Defined.Types.String());
			var httpMethod = methodSymbol.GetHttpMethod();
			switch (httpMethod) {
				case "get":
					if (hasStringReturnType) {
						builder.WithMultiPartName("this", "doGetStringAsync");
					} else {
						builder.WithMultiPartName("this", "doGetAsync").AddGenericArgument(returnType);
					}
					break;
				case "post":
					if (hasStringReturnType) {
						builder.WithMultiPartName("this", "doPostStringAsync");
					} else {
						builder.WithMultiPartName("this", "doPostAsync");
						builder.AddGenericArgument(returnType);
					}
					break;
				case "patch":
					if (hasStringReturnType) {
						builder.WithMultiPartName("this", "doPatchStringAsync");
					} else {
						builder.WithMultiPartName("this", "doPatchAsync");
						builder.AddGenericArgument(returnType);
					}
					break;
				case "put":
					if (hasStringReturnType) {
						builder.WithMultiPartName("this", "doPutStringAsync");
					} else {
						builder.WithMultiPartName("this", "doPutAsync");
						builder.AddGenericArgument(returnType);
					}
					break;
				case "delete":
					builder.WithMultiPartName("this", "doDeleteAsync");
					break;
			}
			// add relativeUrl parameter
			builder.AddArgument(new IdentifierNameExpression("relativeUrl"));
			// add from body parameter if it exists
			var fromBodyParameter = methodSymbol.Parameters.FirstOrDefault(x => x.HasAttribute("Microsoft.AspNetCore.Mvc.FromBodyAttribute"));
			if (fromBodyParameter != null) {
				builder.AddGenericArgument(this.typeConverter.Convert(fromBodyParameter.Type));
				builder.AddArgument(new IdentifierNameExpression(fromBodyParameter.Name.CamelCase()));
			} else if (httpMethod == "post" || httpMethod == "put" || httpMethod == "patch") {
				builder.AddGenericArgument(Defined.Types.String());
				builder.AddArgument(new StringLiteralExpression(""));
			}
			// build query string
			var queryParam = BuildQueryParam(methodSymbol);
			builder.AddArgument(queryParam);

			if (settings.UsePromise && hasVoidReturnType) {
				return builder.Build();
			} else {
				return new ScopedVariableExpressionBuilder()
					.IsConstant().WithName("result").WithExpression(builder.Build())
					.Add(() => new ReturnExpression(new IdentifierNameExpression("result")))
					.BuildAll();
			}
		}

		public IExpression BuildQueryParam(IMethodSymbol methodSymbol) {
			var route = methodSymbol.GetRoute();
			var jsonValues = new List<JsonPropertyExpression>();
			foreach (var param in methodSymbol.Parameters) {
				if (param.TryGetAttribute(My.FromRouteAttributeClassName, out _)) {
					continue;
				} else if (param.TryGetAttribute(My.FromBodyAttributeClassName, out _)) {
					continue;
				} else {
					string queryName = string.Empty;
					if (param.TryGetAttribute(My.FromQueryAttributeClassName, out var fromQueryAttrib)) {
						queryName = fromQueryAttrib.NamedArguments.Where(x => x.Key == "Name").FirstOrDefault().Value.Value as string;
					}
					if (string.IsNullOrEmpty(queryName)) {
						queryName = param.Name.CamelCase();
					}
					jsonValues.Add(new JsonPropertyExpression(queryName, new IdentifierNameExpression(param.Name.CamelCase())));
				}
			}
			return new JsonValueExpression(jsonValues);
		}
	}
}

using Albatross.CodeGen.Syntax;
using System;
using System.Collections.Generic;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public class InvocationExpressionBuilder : ExpressionBuilder<InvocationExpression> {
		private bool useAwaitOperator;
		private bool setTerminate;
		private IIdentifierNameExpression? name;
		private readonly List<IExpression> arguments = new List<IExpression>();
		private readonly List<ITypeExpression> genericArguments = new List<ITypeExpression>();

		public InvocationExpressionBuilder WithName(string name) {
			this.name = new IdentifierNameExpression(name);
			return this;
		}
		public InvocationExpressionBuilder WithMultiPartName(params string[] names) {
			this.name = new MultiPartIdentifierNameExpression(names);
			return this;
		}
		public InvocationExpressionBuilder AddGenericArgument(ITypeExpression typeExpression) {
			this.genericArguments.Add(typeExpression);
			return this;
		}
		public InvocationExpressionBuilder SetTerminate(bool set) {
			setTerminate = set;
			return this;
		}

		public InvocationExpressionBuilder AddArgument(IExpression expression) {
			this.arguments.Add(expression);
			return this;
		}
		public InvocationExpressionBuilder Await(bool useAwait = true) {
			this.useAwaitOperator = useAwait;
			return this;
		}
		public override InvocationExpression Build() {
			return new InvocationExpression() {
				UseAwaitOperator = useAwaitOperator,
				Identifier = name ?? throw new InvalidOperationException("Name is not set"),
				ArgumentList = new ListOfSyntaxNodes<IExpression>(arguments),
				GenericArguments = new ListOfSyntaxNodes<ITypeExpression>(genericArguments),
				Terminate = setTerminate,
			};
		}
	}
}
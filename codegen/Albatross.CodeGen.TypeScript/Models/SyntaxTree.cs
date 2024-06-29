using Albatross.Collections;
using System;

namespace Albatross.CodeGen.TypeScript.Models {
	public class SyntaxTree {
		void ValidateExpression(Expression expression) {
			if(expression.SyntaxTree != this) {
				throw new InvalidOperationException();
			}
		}

		public MethodCallExpression MethodCall(string name, ArgumentListExpression argumentList) {
			ValidateExpression(argumentList);
				return new MethodCallExpression(this) {
					Name = name,
					ArgumentList = argumentList,
				};
		}
		public AwaitMethodExpression AwaitMethod(MethodCallExpression methodCallExpression) {
			ValidateExpression(methodCallExpression);
			return new AwaitMethodExpression(this) {
				MethodCallExpression = methodCallExpression,
			};
		}
		public TypeExpression Type(string name) {
			return new TypeExpression(this) {
				Name = name,
			};
		}
		public GenericTypeExpression GenericType(string name, ArgumentListExpression argumentList) {
			ValidateExpression(argumentList);
			return new GenericTypeExpression(this){
				Name = name,
				GenericTypeArguments = argumentList,
			};
		}
		public ArgumentListExpression ArgumentList(params Expression[] arguments) {
			arguments.ForEach(ValidateExpression);
			return new ArgumentListExpression(this){
				Arguments = arguments,
			};
		}
	}
}

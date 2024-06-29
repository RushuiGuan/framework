using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.TypeScript.Models {
	public class GenericTypeExpressionBuilder {
		private List<Func<SyntaxTree, Expression>> argumentHandlers = new List<Func<SyntaxTree, Expression>>();
		private Func<string>? nameHandler;


		public GenericTypeExpressionBuilder WithName(string name) {
			nameHandler = () => name;
			return this;
		}
		public GenericTypeExpressionBuilder WithName(Func<string> func) {
			nameHandler = func;
			return this;
		}
		public GenericTypeExpressionBuilder WithArgument(Func<SyntaxTree, Expression> func) {
			argumentHandlers.Add(func);
			return this;
		}
		public GenericTypeExpression Build(SyntaxTree syntaxTree) {
			if (nameHandler == null) {
				throw new ArgumentException("Missing name");
			}
			if (argumentHandlers.Any()) {
				var array = argumentHandlers.Select(x => x(syntaxTree)).ToArray();
				return syntaxTree.GenericType(nameHandler(), syntaxTree.ArgumentList(array));
			} else {
				throw new ArgumentException("Missing generic arguments");
			}
		}
	}
}

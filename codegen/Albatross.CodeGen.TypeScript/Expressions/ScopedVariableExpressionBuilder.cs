using Albatross.CodeGen.Syntax;
using System;

namespace Albatross.CodeGen.TypeScript.Expressions {
	public class ScopedVariableExpressionBuilder : ExpressionBuilder<ScopedVariableExpression> {
		private bool isConstant;
		private string? name;
		private ITypeExpression? type;
		private IExpression? expression;

		public ScopedVariableExpressionBuilder WithName(string name) {
			this.name = name;
			return this;
		}
		public ScopedVariableExpressionBuilder IsConstant(bool isConstant = true) {
			this.isConstant = isConstant;
			return this;
		}
		public ScopedVariableExpressionBuilder WithType(ITypeExpression type) {
			this.type = type;
			return this;
		}

		public ScopedVariableExpressionBuilder WithExpression(IExpression expression) {
			this.expression = expression;
			return this;
		}

		public override ScopedVariableExpression Build()
			=> new ScopedVariableExpression(name ?? throw new InvalidOperationException("Variable name has not been set")) {
				IsConstant = isConstant,
				Type = this.type,
				Assignment = expression,
			};
	}
}
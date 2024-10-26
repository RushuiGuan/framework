using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeGen.Syntax {
	public abstract class ExpressionBuilder<T> : ExpressionBuilder where T : IExpression {
		public abstract T Build();
		protected sealed override IExpression InternalBuild() => this.Build();
	}

	public abstract class ExpressionBuilder {
		private Queue<Func<IExpression>> queue;

		public ExpressionBuilder() {
			this.queue = new Queue<Func<IExpression>>();
			queue.Enqueue(this.InternalBuild);
		}

		public Builder Next<Builder>() where Builder : ExpressionBuilder, new() {
			var next = new Builder();
			this.queue.Enqueue(next.InternalBuild);
			next.queue = this.queue;
			return next;
		}
		public ExpressionBuilder Add(ExpressionBuilder builder) {
			this.queue.Enqueue(builder.InternalBuild);
			return this;
		}
		public ExpressionBuilder Add(Func<IExpression> func) {
			this.queue.Enqueue(func);
			return this;
		}
		protected abstract IExpression InternalBuild();
		public CompositeExpression BuildAll() => new CompositeExpression(this.queue.Select(x => x()).ToArray());
	}
}
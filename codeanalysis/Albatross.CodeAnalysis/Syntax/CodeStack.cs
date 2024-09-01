using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeAnalysis.Syntax {
	public class CodeStack {
		public class Scope : IDisposable {
			CodeStack parent;
			public Scope(CodeStack parent) { this.parent = parent; }
			public void Dispose() { parent.End(); }
		}

		Stack<INode> stack = new Stack<INode>();
		Stack<INode> seekStack = new Stack<INode>();

		public CodeStack Begin() {
			stack.Push(new NoOpNodeBuilder());
			return this;
		}
		public CodeStack Begin(INodeBuilder builder) {
			stack.Push(builder);
			return this;
		}
		public CodeStack Complete(INodeBuilder builder) {
			stack.Push(builder);
			this.End();
			return this;
		}
		public CodeStack To(INodeBuilder builder, bool end= true) {
			var localStack = new Stack<INode>();
			var count = 0;
			while (this.stack.Any()) {
				var peek = this.stack.Peek();
				if (peek is INodeContainer container) {
					localStack.Push(this.stack.Pop());
				} else if (peek is EndNode) {
					localStack.Push(this.stack.Pop());
					count++;
				} else {
					if (count > 0) {
						localStack.Push(this.stack.Pop());
						count--;
					} else {
						break;
					}
				}
			}
			this.stack.Push(builder);
			while (localStack.Any()) {
				this.stack.Push(localStack.Pop());
			}
			if (end) {
				this.End();
			}
			return this;
		}
		public Scope NewScope() {
			return new Scope(this);
		}
		public CodeStack With(params INodeContainer[] nodes) {
			foreach (var node in nodes) {
				stack.Push(node);
			}
			return this;
		}
		public CodeStack With(params SyntaxNode[] nodes) {
			foreach (var node in nodes) {
				stack.Push(new NodeContainer(node));
			}
			return this;
		}
		public CodeStack Append(Action<CodeStack> action) {
			action(this);
			return this;
		}
		public CodeStack End() {
			stack.Push(new EndNode());
			return this;
		}

		public CodeStack Seek(Func<INodeBuilder, bool> predicate) {
			if (seekStack.Any()) {
				throw new ArgumentException("A current seek is in progress");
			}
			while (stack.Any()) {
				var node = stack.Pop();
				if (node is INodeBuilder builder && predicate(builder)) {
					stack.Push(builder);
					return this;
				} else {
					seekStack.Push(node);
				}
			}
			throw new ArgumentException("The specific NodeBuilder was not found");
		}
		public CodeStack EndSeek() {
			while (seekStack.Any()) {
				stack.Push(seekStack.Pop());
			}
			return this;
		}
		public string Build() {
			if (!stack.Any()) { throw new ArgumentException("Stack is empty"); }
			var buildStack = new Stack<INode>();
			do {
				var top = stack.Pop();
				if (top is INodeBuilder builder) {
					var nodes = buildStack.PopUntil(x => x is EndNode, out var lastNode).ToArray();
					if (builder is NoOpNodeBuilder) {
						foreach (var node in nodes) {
							buildStack.Push(node);
						}
					} else {
						var result = builder.Build(nodes.Cast<INodeContainer>().Select(x => x.Node).ToArray());
						buildStack.Push(new NodeContainer(result));
					}
				} else {
					buildStack.Push(top);
				}
			} while (stack.Any());
			var sb = new StringBuilder();
			foreach (var node in buildStack) {
				if (node is INodeContainer container) {
					sb.AppendLine(container.ToString());
				} else {
					throw new InvalidOperationException($"Stack item of type {node.GetType().Name} is not expected.  Only {typeof(INodeContainer).Name} is expected");
				}
			}
			return sb.ToString();
		}
		StatementSyntax CreateStatement(SyntaxNode node) {
			switch (node) {
				case VariableDeclarationSyntax variableDeclarationSyntax:
					return SyntaxFactory.LocalDeclarationStatement(variableDeclarationSyntax);
				case ExpressionSyntax expressionSyntax:
					return SyntaxFactory.ExpressionStatement(expressionSyntax);
				default:
					throw new NotSupportedException();
			}
		}
	}
}
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeAnalysis.Syntax {
	public class CodeStack {
		Stack<INode> stack = new Stack<INode>();
		Stack<INode> seekStack = new Stack<INode>();

		public CodeStack Begin(INodeBuilder builder) {
			stack.Push(builder);
			return this;
		}
		public CodeStack End(bool asStatement = false) {
			stack.Push(new EndNode(asStatement));
			return this;
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
					var result = builder.Build(nodes.Cast<INodeContainer>().Select(x => x.Node).ToArray());
					if (((EndNode)lastNode).AsStatement) {
						result = CreateStatement(result);
					}
					buildStack.Push(new NodeContainer(result));
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
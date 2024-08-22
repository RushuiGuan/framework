using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis {
	public class CodeStack {
		Stack<INode> stack = new Stack<INode>();
		public CodeStack Begin(INodeBuilder builder) {
			this.stack.Push(builder);
			return this;
		}
		public CodeStack End() {
			var localStack = new Stack<SyntaxNode>();
			INode? item = null;
			while (this.stack.Any()) {
				item = this.stack.Pop();
				if (item is INodeContainer container) {
					localStack.Push(container.Node);
				} else {
					break;
				}
			}
			if (item == null) {
				throw new InvalidOperationException("Stack is empty");
			} else if (!(item is INodeBuilder)) {
				throw new InvalidOperationException("Expression builder is not found");
			}
			var result = ((INodeBuilder)item).Build(localStack.ToArray());
			this.stack.Push(new NodeContainer(result));
			return this;
		}
		public CodeStack With(params INodeContainer[] nodes) {
			foreach (var node in nodes) {
				this.stack.Push(node);
			}
			return this;
		}
		public string Build() {
			var item = this.stack.Peek();
			if (item is NodeContainer container) {
				return container.Node.NormalizeWhitespace(indentation: "\t").ToFullString();
			} else {
				throw new InvalidOperationException("Top of stack doesn't have a node container");
			}
		}
	}
}

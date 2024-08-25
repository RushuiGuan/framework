using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Albatross.CodeAnalysis {
	public class CodeStack {
		Stack<INode> stack = new Stack<INode>();
		Stack<INode> seekStack = new Stack<INode>();

		public CodeStack Begin(INodeBuilder builder) {
			this.stack.Push(builder);
			return this;
		}
		public CodeStack End() {
			this.stack.Push(new EndNode());
			return this;
		}
		public CodeStack With(params INodeContainer[] nodes) {
			foreach (var node in nodes) {
				this.stack.Push(node);
			}
			return this;
		}
		public CodeStack With(params SyntaxNode[] nodes) {
			foreach (var node in nodes) {
				this.stack.Push(new NodeContainer(node));
			}
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
			while (this.seekStack.Any()) {
				this.stack.Push(this.seekStack.Pop());
			}
			return this;
		}
		public string Build() {
			if (!stack.Any()) { throw new ArgumentException("Stack is empty"); }
			var buildStack = new Stack<INode>();
			do {
				var top = stack.Pop();
				if (top is INodeBuilder builder) {
					var nodes = buildStack.PopUntil(x => x is IEndNode, true);
					var result = builder.Build(nodes.Cast<INodeContainer>().Select(x => x.Node).ToArray());
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
	}
}
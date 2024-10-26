using Microsoft.CodeAnalysis;
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
		public string? FileName { get; set; }

		public CodeStack Begin(INodeBuilder? builder = null) {
			if (builder == null) {
				stack.Push(new NoOpNodeBuilder());
			} else {
				stack.Push(builder);
			}
			return this;
		}
		public CodeStack Complete(INodeBuilder builder) {
			stack.Push(builder);
			this.End();
			return this;
		}
		public CodeStack To(INodeBuilder builder) {
			ToNewBegin(builder);
			this.End();
			return this;
		}
		public CodeStack ToNewBegin(INodeBuilder builder) {
			var localStack = new Stack<INode>();
			while (this.stack.Any()) {
				var peek = this.stack.Peek();
				if (peek is INodeContainer container) {
					localStack.Push(this.stack.Pop());
				} else {
					break;
				}
			}
			this.stack.Push(builder);
			while (localStack.Any()) {
				this.stack.Push(localStack.Pop());
			}
			return this;
		}
		public Scope ToNewScope(INodeBuilder builder) {
			ToNewBegin(builder);
			return new Scope(this);
		}

		public Scope NewScope(INodeBuilder? builder = null) {
			this.Begin(builder);
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
		public CodeStack End() {
			// pop until we found a builder
			var nodes = stack.PopUntil(x => x is INodeBuilder, out var lastNode).ToArray();
			if (lastNode is NoOpNodeBuilder) {
				foreach (var node in nodes) {
					stack.Push(node);
				}
			} else if (lastNode is INodeBuilder builder) {
				var result = builder.Build(nodes.Cast<INodeContainer>().Select(x => x.Node));
				stack.Push(new NodeContainer(result));
			} else {
				throw new InvalidOperationException($"CodeStack is missing a builder therefore out of balance.  Please double check your scopes");
			}
			return this;
		}

		public IEnumerable<INodeContainer> Finalize() {
			foreach (var item in stack.Reverse()) {
				if (item is INodeContainer container) {
					yield return container;
				} else {
					throw new InvalidOperationException($"CodeStack is missing a end call therefore out of balance.  Please double check your scopes");
				}
			}
		}
		public string Build() {
			var nodes = Finalize();
			var sb = new StringBuilder();
			foreach (var node in nodes) {
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
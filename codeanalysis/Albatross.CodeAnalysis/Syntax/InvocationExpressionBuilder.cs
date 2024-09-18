﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Albatross.CodeAnalysis.Syntax {
	/// <summary>
	/// Generate a <see cref="InvocationExpressionSyntax"/> instance.  Expects the following parameters
	/// * <see cref="ExpressionSyntax"/> as the first parameter
	/// * <see cref="SimpleNameSyntax"/> as subsequent parameters
	/// * <see cref="ArgumentListSyntax"/> Optionally as the last parameter
	/// </summary>
	public class InvocationExpressionBuilder : INodeBuilder {
		IdentifierNameSyntax? identifier;
		bool await;
		public InvocationExpressionBuilder() { }
		public InvocationExpressionBuilder(string name) : this(new IdentifierNode(name)) { }
		public InvocationExpressionBuilder(IdentifierNode identifier) {
			this.identifier = identifier.Identifier;
		}

		public INodeBuilder Await() {
			this.await = true;
			return this;
		}


		public SyntaxNode Build(IEnumerable<SyntaxNode> elements) {
			var array = elements.ToArray();
			var nameParameters = new List<SyntaxNode>();
			ArgumentListSyntax argumentList = SyntaxFactory.ArgumentList();
			for (int i = 0; i < array.Length; i++) {
				if (i == array.Length - 1 && array[i] is ArgumentListSyntax argumentListSyntax) {
					argumentList = argumentListSyntax;
				} else {
					nameParameters.Add(array[i]);
				}
			}
			if (this.identifier != null) {
				nameParameters.Add(this.identifier);
			}
			if(nameParameters.Count == 0) {
				throw new ArgumentException($"The {nameof(InvocationExpressionBuilder)} requires at least one {nameof(SimpleNameSyntax)} parameter");
			}

			var name = (ExpressionSyntax)new MemberAccessBuilder().Build(nameParameters);
			var syntax = SyntaxFactory.InvocationExpression(name).WithArgumentList(argumentList);
			if (await) {
				return SyntaxFactory.AwaitExpression(syntax);
			}
			return syntax;
		}
	}
}
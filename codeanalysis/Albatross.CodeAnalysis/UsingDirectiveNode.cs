using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeAnalysis {
	public class UsingDirectiveNode : NodeContainer {
		public UsingDirectiveNode(string name) 
			: base(SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(name))) {
		}
	}
}

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace Albatross.Messaging.CodeGen {
	public class ApiControllerClassWalker: CSharpSyntaxWalker {
		private readonly SemanticModel semanticModel;
		public List<INamedTypeSymbol> Result { get; } = new List<INamedTypeSymbol>();

		public ApiControllerClassWalker(SemanticModel semanticModel) {
			this.semanticModel = semanticModel;
		}
	}
}

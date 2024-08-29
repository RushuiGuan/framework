namespace Albatross.CodeAnalysis.Syntax {
	internal class EndNode : IEndNode {
		public EndNode(bool asStatement) {
			AsStatement = asStatement;
		}

		public bool AsStatement { get; }
	}
}
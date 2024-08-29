namespace Albatross.CodeAnalysis {
	internal class EndNode : IEndNode {
		public EndNode(bool asStatement) {
			AsStatement = asStatement;
		}

		public bool AsStatement { get; }
	}
}
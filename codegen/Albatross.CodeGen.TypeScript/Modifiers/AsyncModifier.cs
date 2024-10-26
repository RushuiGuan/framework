using Albatross.CodeGen.Syntax;

namespace Albatross.CodeGen.TypeScript.Modifiers {
	public record class AsyncModifier : IModifier {
		public AsyncModifier() {
			this.Name = "async";
		}
		public string Name { get; }
	}
}
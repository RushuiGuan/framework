namespace Albatross.CodeGen {
	public interface ICodeModule : ICodeElement {
		public string Name { get; }
		public T Add<T>(T element) where T : ICodeElement;
	}
}

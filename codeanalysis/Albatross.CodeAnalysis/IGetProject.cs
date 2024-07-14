namespace Albatross.CodeAnalysis {
	public interface IGetProject {
		string Path { get; }
	}
	public class GetProject : IGetProject {
		public string Path { get; }
		public GetProject(string path) {
			this.Path = path;
		}
	}
}

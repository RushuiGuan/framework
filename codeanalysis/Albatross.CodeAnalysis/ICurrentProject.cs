namespace Albatross.CodeAnalysis {
	public interface ICurrentProject {
		string Path { get; }
	}
	public class CurrentProject : ICurrentProject {
		public CurrentProject(string path) {
			Path = path;
		}
		public string Path { get; }
	}
}

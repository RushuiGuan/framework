namespace Albatross.CodeAnalysis {
	public interface ICurrentProject {
		string Path { get; }
	}
	public class CurrentProject : ICurrentProject {
		public string Path { get; }
		public CurrentProject(string path) {
			this.Path = path;
		}
	}
}

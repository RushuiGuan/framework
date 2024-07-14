using Microsoft.CodeAnalysis;
using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.MSBuild;

namespace Albatross.CodeAnalysis {
	public class ProjectCompilationFactory : ICompilationFactory {
		private readonly MSBuildWorkspace workspace;
		private readonly ICurrentProject currentProject;

		public ProjectCompilationFactory(MSBuildWorkspace workspace, ICurrentProject currentProject) {
			this.workspace = workspace;
			this.currentProject = currentProject;
		}

		public async Task<Compilation> Get() {
			Project project = await workspace.OpenProjectAsync(currentProject.Path);
			var compilation = await project.GetCompilationAsync();
			if (compilation == null) {
				throw new InvalidOperationException($"Project file {currentProject.Path} doesn't compile");
			} else {
				return compilation;
			}
		}
	}
}

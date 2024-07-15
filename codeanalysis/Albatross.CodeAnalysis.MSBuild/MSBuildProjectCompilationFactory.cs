using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;

namespace Albatross.CodeAnalysis.MSBuild {
	public class MSBuildProjectCompilationFactory: ICompilationFactory {
		private readonly MSBuildWorkspace workspace;
		private readonly ICurrentProject getProject;

		public MSBuildProjectCompilationFactory(MSBuildWorkspace workspace, ICurrentProject getProject) {
			this.workspace = workspace;
			this.getProject = getProject;
		}

		public Compilation Create() {
			var project = workspace.OpenProjectAsync(this.getProject.Path).Result;
			return project.GetCompilationAsync().Result ?? throw new InvalidOperationException($"Unable to create a compilation instance for project {this.getProject.Path}");
		}
	}
}

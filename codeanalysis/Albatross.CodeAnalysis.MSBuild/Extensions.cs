using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.CodeAnalysis.MSBuild {
	public static class Extensions {
		/// <summary>
		/// Create scoped registration for the <see cref="Microsoft.CodeAnalysis.Compilation"/> instance of specified project file so that it can be
		/// injected as dependencies for other classes.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="projectFile"></param>
		/// <returns></returns>
		public static IServiceCollection AddMSBuildProject(this IServiceCollection services, string projectFile) {
			services.AddScoped(provider => MSBuildWorkspace.Create());
			services.AddScoped<ICurrentProject>(provider => new CurrentProject(projectFile));
			services.AddScoped<ICompilationFactory, MSBuildProjectCompilationFactory>();
			services.AddScoped<Compilation>(provider => provider.GetRequiredService<ICompilationFactory>().Create());
			return services;
		}
	}
}

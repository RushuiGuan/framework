using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Albatross.Config.Core {
	public static class ServiceExtension {
		/// <summary>
		/// Init should be set to true for console app or unit test.  aspnetcore will initialize the config objects itself and should be set to false
		/// </summary>
		/// <param name="services"></param>
		/// <param name="entryAssembly"></param>
		/// <returns></returns>
		public static IServiceCollection AddCustomConfig(this IServiceCollection services, Assembly entryAssembly, bool init = false) {
			services.AddSingleton<IGetAssemblyLocation>(new GetEntryAssemblyLocation(entryAssembly));
			services.AddConfig<ProgramSetting, GetProgramSetting>();
			services.AddScoped<GetDbConfigValue>();
			services.AddSingleton<IGetConfigValue, GetJsonConfigValueByDotnetCoreConfiguration>();
			if (init) {
				services.AddSingleton<IConfiguration>(provider => {
					string basePath = System.IO.Path.GetDirectoryName(entryAssembly.Location);
					var builder = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("appsettings.json", false, true);
					IConfigurationRoot root = builder.Build();
					return root;
				});
			}
			return services;
		}

		/// <summary>
		/// Registration for the Configuration Class C and its factory class F 
		/// F is registered as a singleton and C is registered as scope
		/// Interface IConfig&lt;C&gt; is also registered.
		/// Besides creation, the factory class should invoke validation.  It is critical to validate configuration data.  A misspelled property name in the appsettings.json file could lead to null config values.
		/// </summary>
		/// <typeparam name="C">The configuration class</typeparam>
		/// <typeparam name="F">The factory concrete class</typeparam>
		/// <param name="services">The ServiceCollection instance</param>
		/// <returns>The service collection instance</returns>
		public static IServiceCollection AddConfig<C, F>(this IServiceCollection services) where C : class, new() where F : class, IGetConfig<C>{
			services.AddSingleton<F>();
			services.AddSingleton<IGetConfig<C>>(provider => provider.GetRequiredService<F>());
			services.AddScoped<C>(provider=>provider.GetRequiredService<F>().Get());
			return services;
		}
	}
}

using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Config {
	public static class Extension {

		public static string GetAssemblyLocation(this Type type) {
			string codebase = new Uri(type.Assembly.CodeBase).LocalPath;
			return System.IO.Path.GetDirectoryName(codebase);
		}

		public static string GetWorkingDirectory() => System.Environment.CurrentDirectory;

		/// <summary>
		/// Registration for the Configuration Class C and its factory class F 
		/// F is registered as a singleton and C is registered as scope
		/// Interface IConfig&lt;C&gt; is also registered.
		/// Besides creation, the factory class should invoke validation.  It is critical to validate configuration data.  A misspelled property name in the appsettings.json file could lead to null config values.
		/// </summary>
		/// <typeparam name="ConfigType">The configuration class</typeparam>
		/// <typeparam name="FactoryType">The factory concrete class</typeparam>
		/// <param name="services">The ServiceCollection instance</param>
		/// <returns>The service collection instance</returns>
		public static IServiceCollection AddConfig<ConfigType, FactoryType>(this IServiceCollection services, bool singleton = false) where ConfigType : class, new() where FactoryType : class, IGetConfig<ConfigType> {
			services.AddSingleton<FactoryType>();
			services.AddSingleton<IGetConfig<ConfigType>>(provider => provider.GetRequiredService<FactoryType>());
			if (singleton) {
				services.AddSingleton<ConfigType>(provider => provider.GetRequiredService<FactoryType>().Get());
			} else {
				services.AddScoped<ConfigType>(provider => provider.GetRequiredService<FactoryType>().Get());
			}
			return services;
		}
		public static string GetEndPoint(this IConfiguration configuration, string name) => configuration.GetSection($"endpoints:{name}").Value;
	}
}
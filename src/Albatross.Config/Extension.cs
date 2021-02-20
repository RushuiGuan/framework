using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

			services.TryAddSingleton<FactoryType>();
			services.TryAddSingleton<IGetConfig<ConfigType>>(provider => provider.GetRequiredService<FactoryType>());
			if (singleton) {
				services.TryAddSingleton<ConfigType>(provider => provider.GetRequiredService<FactoryType>().Get());
			} else {
				services.TryAddScoped<ConfigType>(provider => provider.GetRequiredService<FactoryType>().Get());
			}
			return services;
		}

		const string Slash = "/";
		/// <summary>
		/// For C# the HttpClient class will remove any relative path if the BaseUrl does not end with a slash.  For example: http://localhost/beezy will becomes 
		/// http://localhost unless base url is set as http://localhost/beezy/
		/// 
		/// For the request url, if it starts with a slash, it will be considered as a root url.  By default, we shouldn't use any slash in the request url.
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetEndPoint(this IConfiguration configuration, string name) {
			string value = configuration.GetSection($"endpoints:{name}").Value;
			if (!value.EndsWith(Slash)) {
				value = value + Slash;
			}
			return value;
		}
	}
}
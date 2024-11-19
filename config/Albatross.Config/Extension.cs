using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Albatross.Config {
	// the class name Extension cannot be renamed to Extensions (standard) because of backward compability reason.
	public static class Extension {
		private static ConfigType CreateNValidate<ConfigType>(IConfiguration configuration) where ConfigType : ConfigBase {
			var cfg = (ConfigType)Activator.CreateInstance(typeof(ConfigType), configuration) 
				?? throw new InvalidOperationException($"Failed to create instance of {typeof(ConfigType).Name}");
			cfg.Validate();
			return cfg;
		}

		/// <summary>
		/// Registration for the Configuration Class C.  C has to have the base class of <see cref="ConfigBase"/> and it also requires a constructor with
		/// a single parameter of type <see cref="IConfiguration"/>
		/// The call will run the validate method right after the object creation.  It is critical to validate configuration data.
		/// A misspelled property name in the appsettings.json file could lead to null config values.
		/// </summary>
		/// <typeparam name="ConfigType">The configuration class</typeparam>
		/// <param name="services">The ServiceCollection instance</param>
		/// <returns>The service collection instance</returns>
#if NET8_0_OR_GREATER
		public static IServiceCollection AddConfig<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] ConfigType>(this IServiceCollection services, bool singleton = true) where ConfigType : ConfigBase {
#else
		public static IServiceCollection AddConfig<ConfigType>(this IServiceCollection services, bool singleton = true) where ConfigType : ConfigBase {
#endif
			if (singleton) {
				services.TryAddSingleton<ConfigType>(provider => CreateNValidate<ConfigType>(provider.GetRequiredService<IConfiguration>()));
			} else {
				services.TryAddScoped<ConfigType>(provider => CreateNValidate<ConfigType>(provider.GetRequiredService<IConfiguration>()));
			}
			return services;
		}

		const string Slash = "/";
		/// <summary>
		/// For C# the HttpClient class will remove any relative path if the BaseUrl does not end with a slash.  For example: http://localhost/beezy will becomes 
		/// http://localhost unless base url is set as http://localhost/beezy/
		/// For the request url, if it starts with a slash, it will be considered as a root url.  By default, we shouldn't use any slash in the request url.
		/// The call will append Slack '/' to the endpoint by default if it doesn't already end with it.  If this behavior is not desired, set ensureTrailingSlash 
		/// to false
		/// </summary>
		/// <param name="configuration"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string? GetEndPoint(this IConfiguration configuration, string name, bool ensureTrailingSlash = true) {
			string? value = configuration.GetSection($"endpoints:{name}")?.Value;
			if (value != null && !value.EndsWith(Slash) && ensureTrailingSlash) {
				value = value + Slash;
			}
			return value;
		}
		public static string? GetEndPoint(this IConfiguration configuration, string name) => GetEndPoint(configuration, name, true);

		public static string GetRequiredEndPoint(this IConfiguration configuration, string name, bool ensureTrailingSlash = true) {
			string section = $"endpoints:{name}";
			string? value = configuration.GetSection(section)?.Value;
			if (value != null && !value.EndsWith(Slash) && ensureTrailingSlash) {
				value = value + Slash;
			}
			return value ?? throw new ConfigurationException(section);
		}
		public static string GetRequiredEndPoint(this IConfiguration configuration, string name) => GetRequiredEndPoint(configuration, name, true);

		public static string GetRequiredConnectionString(this IConfiguration configuration, string name) {
			string? value = configuration.GetConnectionString(name);
			return value ?? throw new ConfigurationException($"connectionStrings:{name}");
		}
	}
}
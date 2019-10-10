﻿using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Albatross.Config {
	public static class Extension {
		/// <summary>
		/// Init should be set to true for console app or unit test.  aspnetcore will initialize the config objects itself and should be set to false
		/// </summary>
		/// <param name="services"></param>
		/// <param name="entryAssembly"></param>
		/// <returns></returns>
		public static IServiceCollection AddConfig(this IServiceCollection services, Assembly entryAssembly, IConfiguration configuration=null) {
			if(configuration != null) { services.AddSingleton(configuration); }
			services.AddSingleton<IGetEntryAssemblyLocation>(new GetEntryAssemblyLocation(entryAssembly));
			services.AddConfig<ProgramSetting, GetProgramSetting>();
			return services;
		}

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
		public static IServiceCollection AddConfig<ConfigType, FactoryType>(this IServiceCollection services) where ConfigType : class, new() where FactoryType : class, IGetConfig<ConfigType> {
			services.AddSingleton<FactoryType>();
			services.AddSingleton<IGetConfig<ConfigType>>(provider => provider.GetRequiredService<FactoryType>());
			services.AddScoped<ConfigType>(provider => provider.GetRequiredService<FactoryType>().Get());
			return services;
		}

		public static SetupConfig UseSerilog(this SetupConfig setupConfig) {
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(setupConfig.Configuration)
				.CreateLogger();
			return setupConfig;
		}

		public static SetupConfig RegisterServices(this SetupConfig setupConfig, IServiceCollection services) {
			services.AddSingleton<IConfiguration>(setupConfig.Configuration);
			services.AddSingleton<IGetEntryAssemblyLocation>(setupConfig.GetAssemblyLocation);
			services.AddConfig<ProgramSetting, GetProgramSetting>();
			return setupConfig;
		}
	}
}

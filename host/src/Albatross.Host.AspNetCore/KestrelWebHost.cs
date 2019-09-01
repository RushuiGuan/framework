using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace Albatross.Host.AspNetCore {
	public class KestrelWebHost <T> where T:class {
		private IWebHost Create(string[] args) {
			const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
			string env = System.Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT);

            var config = new ConfigurationBuilder()
				.SetBasePath(new Albatross.Config.GetEntryAssemblyLocation(typeof(T).Assembly).Directory)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange:true)
                .AddJsonFile("hostsettings.json", optional: true)
				.AddJsonFile($"hostsettings.{env}.json", optional: true)
                .Build();

			Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();

			return WebHost.CreateDefaultBuilder(args)
                .UseStartup<T>()
				.UseSerilog()
                .UseConfiguration(config)
				.UseKestrel()
				.Build();
		}

		public void Run(params string[] args) {
			try {
				Create(args).Run();
			}catch(Exception err) {
				Log.Fatal(err, "Host terminated unexpectedly");
			} finally {
				Log.CloseAndFlush();
			}
		}
	}
}
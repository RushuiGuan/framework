using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using Albatross.Config;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Albatross.Host.AspNetCore {
	public class WebHost<T> where T : class {
		private IHostBuilder Create(string[] args) {
			return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseStartup<T>();
				});
		}

		public async Task RunAsync(params string[] args) {
			try {
				Log.Logger = new LoggerConfiguration()
					.Enrich.FromLogContext()
					.WriteTo.Console()
					.CreateLogger();
				await Create(args).Build().RunAsync();
			} catch (Exception err) {
				Log.Fatal(err, "Host terminated unexpectedly");
			} finally {
				Log.CloseAndFlush();
			}
		}
	}
}
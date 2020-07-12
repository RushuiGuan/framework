using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using Albatross.Config;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Albatross.Logging;
using Microsoft.Extensions.Configuration;

namespace Albatross.Host.AspNetCore {
	public class WebHost<T> where T : class {
		private IHostBuilder Create(string[] args) {
			return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureHostConfiguration(cfgHost => {
					cfgHost.SetBasePath(System.IO.Directory.GetCurrentDirectory());
					cfgHost.AddJsonFile("hostsettings.json", true);
				})
				.ConfigureWebHostDefaults(webBuilder => {
					webBuilder.UseStartup<T>();
					webBuilder.PreferHostingUrls(true);
				});
		}

		public virtual async Task RunAsync(params string[] args) {
			using var setup = new SetupSerilog();
			setup.UseConfigFile("serilog.json");
			await Create(args).Build().RunAsync();
		}
	}
}
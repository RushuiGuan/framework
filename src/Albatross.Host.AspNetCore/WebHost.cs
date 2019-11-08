using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using Albatross.Config;
using System.IO;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Albatross.Logging;

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
			using var setup = new SetupSerilog();
			await Create(args).Build().RunAsync();
		}
	}
}
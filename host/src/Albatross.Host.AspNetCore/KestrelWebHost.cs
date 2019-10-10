using Serilog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using Albatross.Config;
using System.IO;

namespace Albatross.Host.AspNetCore {
	public class KestrelWebHost<T> where T : class {
		private IWebHost Create(string[] args) {
			var setup = new SetupConfig(Directory.GetCurrentDirectory());
			setup.UseSerilog();
			return WebHost.CreateDefaultBuilder(args)
				.UseStartup<T>()
				.UseSerilog()
				.UseConfiguration(setup.Configuration)
				.UseKestrel()
				.Build();
		}

		public void Run(params string[] args) {
			try {
				Create(args).Run();
			} catch (Exception err) {
				Log.Fatal(err, "Host terminated unexpectedly");
			} finally {
				Log.CloseAndFlush();
			}
		}
	}
}
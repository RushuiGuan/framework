using Albatross.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Albatross.Commands.Daemon {
	public class MySetup : Setup {
		public MySetup(string[] args) : base(args) {
		}

		public override Setup ConfigureWebHost<Startup>() {
			hostBuilder.ConfigureWebHostDefaults(webBuilder => {
				webBuilder.UseStartup<Startup>();
				webBuilder.PreferHostingUrls(true);
			});
			return this;
		}
	}
}
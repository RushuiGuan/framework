using Albatross.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Albatross.Host.AspNetCore {
	public class HttpSysWebHost {
		public IWebHost Create<T>(string[] args) where T : class {
			var setup = new SetupConfig(Directory.GetCurrentDirectory());

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<T>()
                .UseConfiguration(setup.Configuration)
				.UseHttpSys(options => {
                    options.Authentication.Schemes = AuthenticationSchemes.NTLM | AuthenticationSchemes.Negotiate;
					options.Authentication.AllowAnonymous = true;
				}).Build();
		}
	}
}
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Albatross.Host.AspNetCore {
	public class HttpSysWebHost {
		public IWebHost Create<T>(string[] args) where T : class {
			const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
			string env = System.Environment.GetEnvironmentVariable(ASPNETCORE_ENVIRONMENT);

            var config = new ConfigurationBuilder()
                .AddJsonFile("hostsettings.json", optional: true)
				.AddJsonFile($"hostsettings.{env}.json", optional: true)
				.AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<T>()
                .UseConfiguration(config)
				.UseHttpSys(options => {
                    options.Authentication.Schemes = AuthenticationSchemes.NTLM | AuthenticationSchemes.Negotiate;
					options.Authentication.AllowAnonymous = true;
				}).Build();
		}
	}
}
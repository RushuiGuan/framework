using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Host.AspNetCore {
	public class DefaultIISWebHost {
		public IWebHost Create<T>(string[] args) where T : class {
			return WebHost.CreateDefaultBuilder(args)
				.UseIISIntegration()
				.UseStartup<T>()
				.Build();
		}
	}
}
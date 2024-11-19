using Albatross.CommandLine;
using Albatross.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AotTest {
	public class MySetup : Setup {
		public override void RegisterServices(InvocationContext context, IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(context, configuration, envSetting, services);
			services.AddConfig<MyConfig>();
			services.RegisterCommands();
		}
	}
}

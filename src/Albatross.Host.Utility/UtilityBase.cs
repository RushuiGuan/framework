using Albatross.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Albatross.Host.Utility {
	public abstract class UtilityBase<Option> : IUtility<Option> {
		public TextWriter Out => System.Console.Out;
		public TextWriter Error => System.Console.Error;

		protected Option option;
		protected IServiceProvider Provider { get; private set; }
		protected IConfiguration Configuration { get; private set; }

		public IUtility<Option> Init(Option option) {
			this.option = option;
			ServiceCollection services = new ServiceCollection();
			var setup = new SetupConfig(this.GetType().GetAssemblyLocation()).RegisterServices(services);
			Configuration = setup.Configuration;
			Register(services, Configuration, option);
			Provider = services.BuildServiceProvider();
			return this;
		}

		public abstract void Register(IServiceCollection services, IConfiguration configuration, Option option);
		public abstract int Run();
	}
}

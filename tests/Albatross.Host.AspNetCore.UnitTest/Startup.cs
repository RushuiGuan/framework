using System.Diagnostics;
using Albatross.Config.Core;
using Albatross.Host.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Host.AspNetCore.UnitTest {
	public class Startup : Albatross.Host.AspNetCore.Startup {
		public Startup(IConfiguration configuration) : base(configuration) {
		}
		public override void AddCustomServices(IServiceCollection services) {
			base.AddCustomServices(services);
		}
		public override void Configure(IApplicationBuilder app, IHostingEnvironment env, GlobalExceptionHandler globalExceptionHandler, ProgramSetting program) {
			base.Configure(app, env, globalExceptionHandler, program);
			Debug.Assert(env.ContentRootPath != null);
		}
	}
}
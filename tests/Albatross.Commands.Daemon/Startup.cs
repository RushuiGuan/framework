using Albatross.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Albatross.Commands.Daemon {
	public class Startup : Albatross.Hosting.Startup{

		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Grpc => false;
		public override bool Secured => true;
		public override bool Spa => false;
		public override bool Caching => true;


		public Startup(IConfiguration configuration) : base(configuration) {
		}

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddCommandBus().AddCommand<MyCommand>((cmd, _)=> "default", Extensions.CreateDefaultQueue).AddCommandHandler<MyCommandHandler>();
			services.AddCommandBus().AddCommand<MyCommand2>((_, _) => "improved", Extensions.CreateImprovedQueue).AddCommandHandler<MyCommandHandler2>();

			services.AddDefaultEventPublisher<MyCommandExecuted>();
			services.AddSingleton<IEventSubscription<MyCommandExecuted>, PositionService>();
		}
		public override void Configure(IApplicationBuilder app, ProgramSetting programSetting, EnvironmentSetting environmentSetting, ILogger<Hosting.Startup> logger) {
			base.Configure(app, programSetting, environmentSetting, logger);
		}
	}
}

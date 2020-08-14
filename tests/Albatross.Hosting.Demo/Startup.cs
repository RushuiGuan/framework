using Albatross.Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Hosting.Demo {
	/// <summary>
	/// </summary>
	public class Startup : Albatross.Hosting.Startup {
		public override bool Spa => true;
		public override bool Grpc => true;
		public override bool Swagger => true;
		public override bool WebApi => true;
		public override bool Secured => true;

		public Startup(IConfiguration configuration) : base(configuration) {
		}

		public override void ConfigureServices(IServiceCollection services) {
			base.ConfigureServices(services);
			services.AddAzureADSupport(this.Configuration);
		}

		//protected override void BuildAuthorizationPolicy(AuthorizationPolicyBuilder builder, AuthorizationPolicy policy) {
		//	if (policy.Roles?.Length > 0) {
		//		builder.RequireAzureRole(policy.Roles);
		//	}
		//}
	}
}
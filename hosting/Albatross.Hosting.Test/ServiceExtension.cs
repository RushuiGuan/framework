using Albatross.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Hosting.Test {
	public static class ServiceExtension {
		public static IServiceCollection AddTestPrincipalProvider(this IServiceCollection svc, string authProvider, string account) {
			return svc.AddScoped<IGetCurrentUser>(provider => new GetCurrentTestUser(authProvider, account));
		}
	}
}
using Albatross.Authentication.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Hosting.Test {
	public static class ServiceExtension {
		public static IServiceCollection AddTestPrincipalProvider(this IServiceCollection svc, string provider, string account) {
			return svc.AddSingleton<IGetCurrentUser>(new GetCurrentTestUser(provider, account));
		}
	}
}
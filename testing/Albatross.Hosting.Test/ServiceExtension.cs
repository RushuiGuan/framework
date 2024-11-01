using Albatross.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.DependencyInjection.Testing {
	public static class ServiceExtension {
		/// <summary>
		/// register the test principal provider in such a way that the test provider instance can be retrieved and modified
		/// </summary>
		/// <param name="services"></param>
		/// <param name="authProvider"></param>
		/// <param name="account"></param>
		/// <returns></returns>
		public static IServiceCollection AddTestPrincipalProvider(this IServiceCollection services, string authProvider, string account) {
			services.AddSingleton(provider => new GetCurrentTestUser(authProvider, account));
			services.AddScoped<IGetCurrentUser>(provider => provider.GetRequiredService<GetCurrentTestUser>());
			return services;
		}
	}
}
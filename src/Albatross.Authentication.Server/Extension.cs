using Albatross.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Authentication.Server {
	public static class Extension {
		public static IServiceCollection AddWindowsAuthenticationServer(this IServiceCollection services) {
			services.AddSingleton<IUserProfileService, ActiveDirectoryUserProfileService>();
			services.AddCacheMgmt(typeof(Extension).Assembly);
			return services;
		}


		public static IServiceCollection AddWindowsAuthenticationServerTest(this IServiceCollection services) {
			services.AddWindowsAuthenticationServer();
			services.AddSingleton< ActiveDirectoryUserProfileService>();
			return services;
		}
	}
}

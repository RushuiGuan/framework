using Albatross.Caching;
using Albatross.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Polly.Registry;
using System.Threading.Tasks;

namespace Albatross.Authentication.Server {
	public static class Extension {
		public static IServiceCollection AddWindowsAuthenticationServer(this IServiceCollection services) {
			services.AddSingleton<IUserProfileService, ActiveDirectoryUserProfileService>();
			services.AddCacheMgmt(typeof(Extension).Assembly);
			services.AddConfig<ActiveDirectoryConfig, GetActiveDirectoryConfig>(true);
			return services;
		}


		public static IServiceCollection AddWindowsAuthenticationServerTest(this IServiceCollection services) {
			services.AddWindowsAuthenticationServer();
			services.AddSingleton< ActiveDirectoryUserProfileService>();
			services.AddConfig<ActiveDirectoryConfig, GetActiveDirectoryConfig>(true);
			return services;
		}

		public static async Task InitProfiles(Polly.Registry.IReadOnlyPolicyRegistry<string> registry, IUserProfileService userProfile) {
			var policy = registry.GetAsyncPolicy<User>(nameof(ActiveDirectoryUserProfileCacheMgmt));
			var users = await userProfile.Search();
			foreach(var item in users) {
				await policy.ExecuteAsync(context => Task.Run<User>(()=>item), new Polly.Context(item.Account));
			}
		}

		public static Task InitProfiles(this IApplicationBuilder applicationBuilder) {
			var registry = applicationBuilder.ApplicationServices.GetRequiredService<IReadOnlyPolicyRegistry<string>>();
			var svc = applicationBuilder.ApplicationServices.GetRequiredService<IUserProfileService>();
			return InitProfiles(registry, svc);
		}
	}
}

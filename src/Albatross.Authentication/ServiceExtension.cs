using Albatross.Authentication.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Authentication {
	public static class ServiceExtension {
		public static IServiceCollection AddWindowsPrincipalProvider(this IServiceCollection svc) {
			svc.AddSingleton<IGetCurrentUser, GetCurrentWindowsUser>();
			return svc;
		}

		public static IServiceCollection AddAspNetCorePrincipalProvider(this IServiceCollection svc) {
			svc.AddSingleton<IGetCurrentUser, GetCurrentUserFromHttpContext>();
			svc.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			return svc;
		}
	}
}

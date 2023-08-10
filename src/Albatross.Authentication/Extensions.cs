using Albatross.Authentication.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Authentication {
	public static class Extensions {
		public static IServiceCollection AddWindowsPrincipalProvider(this IServiceCollection svc) {
			svc.AddSingleton<IGetCurrentUser, GetCurrentWindowsUser>();
			return svc;
		}

		public static IServiceCollection AddAspNetCorePrincipalProvider(this IServiceCollection svc) {
			svc.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			svc.AddSingleton<IGetCurrentUser, GetCurrentUserFromHttpContext>();
			return svc;
		}
	}
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Authentication.AspNetCore {
	public static class Extensions {
		public static IServiceCollection AddAspNetCorePrincipalProvider(this IServiceCollection svc) {
			svc.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			svc.AddSingleton<IGetCurrentUser, GetCurrentUserFromHttpContext>();
			return svc;
		}
	}
}
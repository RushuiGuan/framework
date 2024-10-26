using Microsoft.Extensions.DependencyInjection;

namespace Albatross.Authentication.Windows {
	public static class Extensions {
		public static IServiceCollection AddWindowsPrincipalProvider(this IServiceCollection svc) {
			svc.AddSingleton<IGetCurrentUser, GetCurrentWindowsUser>();
			return svc;
		}
	}
}
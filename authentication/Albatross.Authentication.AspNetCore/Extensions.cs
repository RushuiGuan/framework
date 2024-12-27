using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Albatross.Authentication.AspNetCore {
	public static class Extensions {
		public static IServiceCollection AddAspNetCorePrincipalProvider(this IServiceCollection svc) {
			svc.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			svc.AddSingleton<IGetCurrentUser, GetCurrentUserFromHttpContext>();
			return svc;
		}

		const string ClaimType_Preferred_Username = "preferred_username";
		const string ClaimType_Name = "name";

		public static string? GetIdentity(this ClaimsPrincipal user) {
			var name = user.Identity?.Name;
			if (string.IsNullOrEmpty(name)) {
				name = user.FindFirst(ClaimType_Name)?.Value;
				if (string.IsNullOrEmpty(name)) {
					name = user.FindFirst(ClaimType_Preferred_Username)?.Value;
				}
			}
			return name;
		}
		/// <summary>
		/// Normalize the identity to just the user name.  
		/// If the identity is in the form of domain\user, then just return the user name.
		/// If the identity is null or empty, then return "Anonymous"
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string NormalizeIdentity(this string? name) {
			if (!string.IsNullOrEmpty(name)) {
				int i = name.IndexOf('\\');
				if (i >= 0) {
					name = name.Substring(i + 1);
				}
				return name;
			} else {
				return My.Anonymous;
			}
		}
		public static string GetIdentity(this HttpContext? context) {
			var name = context?.User.GetIdentity();
			return name.NormalizeIdentity();
		}
	}
}
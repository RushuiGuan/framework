using Albatross.Authentication.Core;
using Microsoft.AspNetCore.Http;
using System;

namespace Albatross.Authentication {
	public class GetCurrentUserFromHttpContext : IGetCurrentUser {
		public static string GetFromContext(HttpContext? context) {
			string name = context?.User?.Identity?.Name;
			if (string.IsNullOrEmpty(name)) {
				name = context?.User?.FindFirst(ClaimType_Name)?.Value;
			} else if (string.IsNullOrEmpty(name)) {
				name = context?.User?.FindFirst(ClaimType_Preferred_Username)?.Value;
			}
			if (!string.IsNullOrEmpty(name)) {
				int i = name.IndexOf('\\');
				if (i >= 0) {
					name = name.Substring(i + 1);
				}
				return name;
			} else {
				return "Anonymous";
			}
		}

		IHttpContextAccessor httpContextAccessor;
		public GetCurrentUserFromHttpContext(IHttpContextAccessor httpContextAccessor) {
			this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		public string Provider => "httpcontext";
		const string ClaimType_Preferred_Username = "preferred_username";
		const string ClaimType_Name = "name";

		public string Get() => GetFromContext(httpContextAccessor.HttpContext);
	}
}

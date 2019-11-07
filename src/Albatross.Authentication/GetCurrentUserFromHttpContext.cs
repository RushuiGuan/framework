using Albatross.Authentication.Core;
using Microsoft.AspNetCore.Http;
using System;

namespace Albatross.Authentication {
	public class GetCurrentUserFromHttpContext : IGetCurrentUser {
		IHttpContextAccessor httpContextAccessor;
		public GetCurrentUserFromHttpContext(IHttpContextAccessor httpContextAccessor) {
			this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		public string Provider => "httpcontext";
		const string ClaimType_ClientID = "client_id";

		public string Get() {
			string name = httpContextAccessor.HttpContext?.User?.Identity?.Name;
			if (string.IsNullOrEmpty(name)) {
				name = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimType_ClientID)?.Value;
			}
			if (!string.IsNullOrEmpty(name)) {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
				int i = name.IndexOf('\\');
#pragma warning restore CS8602 // Dereference of a possibly null reference.
				if (i >= 0) {
					name = name.Substring(i + 1);
				}
				return name;
			} else {
				return "Anonymous";
			}
		}
	}
}

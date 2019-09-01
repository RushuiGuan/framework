using Albatross.Authentication.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Albatross.Authentication {
	public class GetCurrentUserFromHttpContext : IGetCurrentUser {
		IHttpContextAccessor httpContextAccessor;
		public GetCurrentUserFromHttpContext(IHttpContextAccessor httpContextAccessor) {
			this.httpContextAccessor = httpContextAccessor;
		}

		public string AuthenticationType => "httpcontext";
		const string ClaimType_ClientID = "client_id";

		public string Get() {
			string name = httpContextAccessor.HttpContext.User.Identity.Name;
			if (string.IsNullOrEmpty(name)) {
				name = httpContextAccessor.HttpContext.User.FindFirst(ClaimType_ClientID)?.Value;
			}
			if (!string.IsNullOrEmpty(name)) {
				int i = name.IndexOf('\\');
				if (i >= 0) {
					name = name.Substring(i + 1);
				}
				return name;
			} else {
				return null;
			}
		}
	}
}

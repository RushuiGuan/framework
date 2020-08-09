using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Azure {
	public static class Extension {
		public static AuthorizationPolicyBuilder RequireAzureRole(this AuthorizationPolicyBuilder builder, params string[] roles) {
			if (roles == null) {
				throw new ArgumentNullException(nameof(roles));
			}

			builder.Requirements.Add(new AzureRolesAuthorizationRequirement(roles));
			return builder;
		}
	}
}

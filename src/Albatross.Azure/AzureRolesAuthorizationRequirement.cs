using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Albatross.Azure
{
	public class AzureRolesAuthorizationRequirement : RolesAuthorizationRequirement {
		public AzureRolesAuthorizationRequirement(IEnumerable<string> allowedRoles) : base(allowedRoles) {
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement) {
			if (context.User != null) {
				bool found = false;
				if (requirement.AllowedRoles == null || !requirement.AllowedRoles.Any()) {
					// Review: What do we want to do here?  No roles requested is auto success?
				} else {
					foreach(var item in requirement.AllowedRoles) {
						found = await IsUserInRole(context, item);
						if (found) { break; }
					}
				}
				if (found) {
					context.Succeed(requirement);
				}
			}
		}

		async Task<bool> IsUserInRole(AuthorizationHandlerContext context, string role) {
			if (context.User.IsInRole(role)) {
				return true;
			} else if(context.User.Claims.Any(args=>args.Type == "hasgroups" && args.Value == "true")) {
				return await Task.FromResult(true);
			}
			return false;
		}
	}
}

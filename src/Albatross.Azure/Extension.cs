using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using System;
using System.Net.Http.Headers;

namespace Albatross.Azure {
	public static class Extension {
		public const string ScopeUserRead = "User.Read";
		public const string ScopeDirectoryReadAll = "Directory.Read.All";
		public readonly static string[] GraphApiScopes = new string[] { 
			ScopeUserRead, ScopeDirectoryReadAll,
		};

		public static AuthorizationPolicyBuilder RequireAzureRole(this AuthorizationPolicyBuilder builder, params string[] roles) {
			if (roles == null) {
				throw new ArgumentNullException(nameof(roles));
			}

			builder.Requirements.Add(new AzureRolesAuthorizationRequirement(roles));
			return builder;
		}

		public static IServiceCollection AddAzureADSupport(this IServiceCollection services, IConfiguration configuration) {
			services.AddMicrosoftWebAppAuthentication(configuration)
				.AddMicrosoftWebAppCallsWebApi(configuration, new string[] { ScopeUserRead, ScopeDirectoryReadAll })
				.AddInMemoryTokenCaches();

			services.AddSingleton<IGraphServiceClientFactory, GraphServiceClientFactory>();
			return services;
		}
	}
}

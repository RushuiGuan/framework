using Albatross.Config.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Albatross.Hosting {
	public class SwaggerScope {
		public string Name { get; set; }
		public string Description { get; set; }

		public SwaggerScope(string name, string description) {
			this.Name = name;
			this.Description = description;
		}
	}
	public class AuthorizationPolicy {
		public string Name { get; set; }
		public string[] Roles { get; set; } = new string[0];

		public AuthorizationPolicy(string name) {
			this.Name = name;
		}
	}
	public class AuthorizationSetting : IConfigSetting {
		public const string BearerAuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
		public const string KerborosAuthenticationScheme = NegotiateDefaults.AuthenticationScheme;
		public const string WindowsAuthenticationScheme = HttpSysDefaults.AuthenticationScheme;
		public static readonly string[] SupportedAuthenticationScheme = new string[] {
			BearerAuthenticationScheme,
			KerborosAuthenticationScheme,
			WindowsAuthenticationScheme,
		};

		public const string key = "authorization";

		/// <summary>
		/// Authentication scheme is case sensitive!  bearer or Negotiate
		/// </summary>
		[Required]
		public string Authentication { get; set; } = "NotSupported";

		#region Bearer authentication scheme
		/// <summary>
		/// BaseUrl of the Authorization Server
		/// </summary>
		public string? Authority { get; set; }
		public string? AuthorizeUrl { get; set; }
		public string? TokenUrl { get; set; }
		public string? SwaggerClientId { get; set; }
		public SwaggerScope[] SwaggerScopes { get; set; } = new SwaggerScope[0];
		/// <summary>
		/// ApiResource Name
		/// </summary>
		public string? Audience { get; set; }
		#endregion

		public bool IsBearerAuthentication => string.Equals(Authentication, BearerAuthenticationScheme);
		public bool IsKerborosAuthentication => string.Equals(Authentication, KerborosAuthenticationScheme);
		public bool IsWindowsAuthentication => string.Equals(Authentication, WindowsAuthenticationScheme);


		public void Validate() {
			if (string.IsNullOrEmpty(Authentication)) {
				throw new ConfigurationException(this.GetType(), nameof(Authentication));
			} else if (!SupportedAuthenticationScheme.Contains(Authentication, StringComparer.InvariantCultureIgnoreCase)) {
				throw new NotSupportedException($"Authentication Scheme {Authentication} is not supported.  Authentation Scheme is case sensitive and should be one of the following: {string.Join(',', SupportedAuthenticationScheme)}");
			} else if (IsBearerAuthentication) {
				if (string.IsNullOrEmpty(Authority)) { throw new ConfigurationException(this.GetType(), nameof(Authority)); }
				if (string.IsNullOrEmpty(AuthorizeUrl)) { throw new ConfigurationException(this.GetType(), nameof(AuthorizeUrl)); }
				if (string.IsNullOrEmpty(TokenUrl)) { throw new ConfigurationException(this.GetType(), nameof(TokenUrl)); }
			}
		}

		public void Init(IConfiguration configuration) { }
		public AuthorizationPolicy[] Policies { get; set; } = new AuthorizationPolicy[0];
	}
}
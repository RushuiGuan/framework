using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;

namespace Albatross.Hosting {
	public class SwaggerScope {
		public string Name { get; set; }
		public string Description { get; set; }
	}
	public class AuthorizationPolicy {
		public string Name { get; set; }
		public string[] Roles { get; set; }
	}
	public class AuthorizationSetting : IConfigSetting {
		public const string BearerAuthenticationScheme = "bearer";
		public const string WindowsAuthenticationScheme = "Windows";

		public const string key = "authorization";

		public string Authentication { get; set; }

		#region Bearer authentication scheme
		/// <summary>
		/// BaseUrl of the Authorization Server
		/// </summary>
		public string Authority { get; set; }
		public string AuthorizeUrl { get; set; }
		public string TokenUrl { get; set; }
		public string SwaggerClientId { get; set; }
		public SwaggerScope[] SwaggerScopes { get; set; }
		/// <summary>
		/// ApiResource Name
		/// </summary>
		public string Audience { get; set; }
		#endregion

		public bool IsBearerAuthentication => string.Equals(Authentication, BearerAuthenticationScheme, StringComparison.InvariantCultureIgnoreCase);
		public bool IsWindowsAuthentication => string.Equals(Authentication, WindowsAuthenticationScheme, StringComparison.InvariantCultureIgnoreCase);


		public void Validate() {
			if (string.IsNullOrEmpty(Authentication)) {
				throw new ConfigurationException(this.GetType(), nameof(Authentication));
			} else if (IsBearerAuthentication) {
				if (string.IsNullOrEmpty(Authority)) { throw new ConfigurationException(this.GetType(), nameof(Authority)); }
				if (string.IsNullOrEmpty(AuthorizeUrl)) { throw new ConfigurationException(this.GetType(), nameof(AuthorizeUrl)); }
				if (string.IsNullOrEmpty(TokenUrl)) { throw new ConfigurationException(this.GetType(), nameof(TokenUrl)); }
			}
		}

		public void Init(IConfiguration configuration) {
		}

		public AuthorizationPolicy[] Policies { get; set; }
	}
}
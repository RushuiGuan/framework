using Albatross.Config.Core;
using System.Collections.Generic;

namespace Albatross.Host.AspNetCore {
	public class SwaggerScope {
		public string Name { get; set; }
		public string Description { get; set; }
	}
	public class AuthorizationSetting : IConfigSetting {
		public const string key = "authorization";

		/// <summary>
		/// BaseUrl of the Authorization Server
		/// </summary>
		public string Authority { get; set; }

		public string AuthorizeUrl { get; set; }
		public string TokenUrl { get; set; }

		/// <summary>
		/// Swagger client id.  If not set, it will be set as '<see cref="ProgramSetting.App"/>-swagger'.
		/// </summary>
		public string SwaggerClientId { get; set; }

		/// <summary>
		/// Collection of swagger scopes.  If not set, the default scope name will be the application name appended with "-default"
		/// The application name comes from  <see cref="ProgramSetting.App"/>
		/// </summary>
		public SwaggerScope[] SwaggerScopes { get; set; }

		/// <summary>
		/// ApiResource Name.  If not set, it will be set to the same value as <see cref="ProgramSetting.App"/>.
		/// </summary>
		public string Audience { get; set; }

		public void Validate() {
			if (string.IsNullOrEmpty(Authority)) {
				throw new ConfigurationException(this.GetType(), nameof(Authority));
			}
		}

		public void SetDefault(ProgramSetting programSetting) {
			if (string.IsNullOrEmpty(SwaggerClientId)) { SwaggerClientId = $"{programSetting.App}-swagger"; }
			if (SwaggerScopes?.Length == null || SwaggerScopes.Length == 0) {
				SwaggerScopes = new SwaggerScope[] { 
					new SwaggerScope{ 
						 Name = $"{programSetting.App}-default",
						 Description = $"default scope - {programSetting.App}",
					}
				};
			}
		}
	}
}
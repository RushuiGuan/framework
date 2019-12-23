using Albatross.Config.Core;
using System.Collections.Generic;

namespace Albatross.Host.AspNetCore {
	public class AuthorizationSetting : IConfigSetting {
		public const string key = "authorization";

		/// <summary>
		/// BaseUrl of the Authorization Server
		/// </summary>
		public string Authority { get; set; }

		public string AuthorizeUrl => $"{Authority}/connect/authorize";
		public string TokenUrl => $"{Authority}/connect/token";

		/// <summary>
		/// Swagger client id.  If not set, it will be set as '<see cref="ProgramSetting.App"/>-swagger'.
		/// </summary>
		public string SwaggerClientId { get; set; }

		/// <summary>
		/// Key value pair of Swagger Scope name and description.  Multiple scopes can be specified.  If not set, the default scope name will be the application name appended with "-default"
		/// The application name comes from  <see cref="ProgramSetting.App"/>
		/// </summary>
		public Dictionary<string, string> SwaggerScopes { get; set; }

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
			if (string.IsNullOrEmpty(SwaggerClientId)) {
				SwaggerClientId = $"{programSetting.App}-swagger";
			}
			if (SwaggerScopes == null || SwaggerScopes.Count == 0) {
				SwaggerScopes = new Dictionary<string, string> { { $"{programSetting.App}-default", $"Default Scope for {programSetting.App}" } };
			}
			if (string.IsNullOrEmpty(Audience)) { Audience = programSetting.App; }
		}
	}
}
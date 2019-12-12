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
		/// Swagger Scopes.  If not set, it will be set using the default scope '<see cref="ProgramSetting.App"/>-default'.
		/// </summary>
		public string[] SwaggerScopes { get; set; }

		public Dictionary<string, string> SwaggerScopeDictionary {
			get {
				Dictionary<string, string> dict = new Dictionary<string, string>();
				if (SwaggerScopes != null) {
					foreach (var item in SwaggerScopes) { dict.Add(item, string.Empty); }
				}
				return dict;
			}
		}

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
			if(SwaggerScopes == null||SwaggerScopes.Length == 0) {
				SwaggerScopes = new string[] {
					$"{programSetting.App}-default",
				};
			}
			if (string.IsNullOrEmpty(Audience)) { Audience = programSetting.App; }
		}
	}
}
using Albatross.Config.Core;

namespace Albatross.Host.AspNetCore {
	public class AuthorizationSetting : IConfigSetting {
		public const string key = "authorization";

		/// <summary>
		/// BaseUrl of the Authorization Server
		/// </summary>
		public string Authority { get; set; }

		public string AuthorizeUrl => $"{Authority}/connect/authorize";
		public string TokenUrl => $"{Authority}/connect/token";

		public string SwaggerClientId { get; set; }
		public string[] SwaggerScopes { get; set; }

		/// <summary>
		/// ApiResource Name
		/// </summary>
		public string Audience { get; set; }

		public void Validate() {
			if (string.IsNullOrEmpty(Authority)) {
				throw new ConfigurationException(this.GetType(), nameof(Authority));
			}
			if (string.IsNullOrEmpty(Audience)) {
				throw new ConfigurationException(this.GetType(), nameof(Audience));
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
		}
	}
}
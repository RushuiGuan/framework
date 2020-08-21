using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.WebClient {
	public class ClientAuthorizationSetting : IConfigSetting {
		public const string Key = "webclient-authorization";

		public string TokenEndPoint { get; set; }
		public string ClientID { get; set; }
		public string ClientSecret { get; set; }
		public string[] Scopes { get; set; }

		public void Init(IConfiguration configuration) { }

		public void Validate() {
			if (string.IsNullOrEmpty(TokenEndPoint)) throw new ConfigurationException(typeof(ClientAuthorizationSetting), nameof(TokenEndPoint));
			if (string.IsNullOrEmpty(ClientID)) throw new ConfigurationException(typeof(ClientAuthorizationSetting), nameof(ClientID));
			if (string.IsNullOrEmpty(ClientSecret)) throw new ConfigurationException(typeof(ClientAuthorizationSetting), nameof(ClientSecret));
			if (!(Scopes?.Length > 0)) {
				throw new ConfigurationException(typeof(ClientAuthorizationSetting), nameof(Scopes));
			}
		}
	}
}
using Albatross.Config;
using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.WebClient {
	public class GetClientAuthorizationSetting : GetConfig<ClientAuthorizationSetting> {
		public GetClientAuthorizationSetting(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => ClientAuthorizationSetting.Key;
	}
}

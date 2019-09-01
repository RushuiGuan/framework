using Albatross.Config;
using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.WebClient {
	public class GetClientAuthorizationSetting : GetConfig<ClientAuthorizationSetting> {
		public GetClientAuthorizationSetting(IGetConfigValue getConfigValue) : base(getConfigValue) {
		}

		protected override string Key => ClientAuthorizationSetting.Key;
	}
}

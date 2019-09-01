using Albatross.Config;
using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Host.AspNetCore {
	public class GetAuthorizationSetting : GetConfig<AuthorizationSetting> {
		public GetAuthorizationSetting(IGetConfigValue getConfigValue) : base(getConfigValue) {
		}
		protected override string Key => AuthorizationSetting.key;
	}
}

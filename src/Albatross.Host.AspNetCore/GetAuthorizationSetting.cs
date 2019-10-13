using Albatross.Config;
using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Host.AspNetCore {
	public class GetAuthorizationSetting : GetConfig<AuthorizationSetting> {
		public GetAuthorizationSetting(IConfiguration getConfigValue) : base(getConfigValue) {}
		protected override string Key => AuthorizationSetting.key;
	}
}

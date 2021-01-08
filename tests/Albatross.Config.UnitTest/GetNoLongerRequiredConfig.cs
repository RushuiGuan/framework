using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	public class NoLongerRequiredConfig { }
	public class GetNoLongerRequiredConfig : GetConfig<NoLongerRequiredConfig> {

		public GetNoLongerRequiredConfig(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => "important";
	}
}

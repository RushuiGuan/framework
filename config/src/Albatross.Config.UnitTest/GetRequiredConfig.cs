using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	public class GetRequiredConfig : GetConfig<string> {

		public GetRequiredConfig(IGetConfigValue getConfigValue) : base(getConfigValue) {
		}

		protected override string Key => "important";
	}
}

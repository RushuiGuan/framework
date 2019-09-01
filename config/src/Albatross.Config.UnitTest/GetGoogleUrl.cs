using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	public class GetGoogleUrl : GetConfig<string> {

		public GetGoogleUrl(IGetConfigValue getConfigValue) : base(getConfigValue) {
		}

		protected override string Key => "google";
	}
}

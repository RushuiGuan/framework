using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	public class GetChangeTest : GetConfig<int> {

		public GetChangeTest(IGetConfigValue getConfigValue) : base(getConfigValue) {
		}

		protected override string Key => "change-test";
	}
}

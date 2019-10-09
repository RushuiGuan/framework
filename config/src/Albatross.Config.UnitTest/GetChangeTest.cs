using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	public class GetChangeTest : GetConfig<int> {

		public GetChangeTest(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => "change-test";
	}
}

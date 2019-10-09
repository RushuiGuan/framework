using Albatross.Config.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	public class GetGoogleUrl : GetConfig<string> {

		public GetGoogleUrl(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => "google";
	}
}

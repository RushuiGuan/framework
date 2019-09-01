using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Config.UnitTest {
	public class GetConfigManagementDatabaseConnection : GetConfig<string> {

		public GetConfigManagementDatabaseConnection(IGetConfigValue getConfigValue) : base(getConfigValue) {
		}

		protected override string Key => "configManagementDatabaseConnection";
	}
}

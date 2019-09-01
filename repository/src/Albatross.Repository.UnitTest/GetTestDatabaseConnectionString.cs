using Albatross.Config;
using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest
{
    public class GetTestDatabaseConnectionString : GetConfig<string>
    {
		public GetTestDatabaseConnectionString(IGetConfigValue getConfigValue) : base(getConfigValue) { }

		protected override string Key => "connectionString.test";
	}
}

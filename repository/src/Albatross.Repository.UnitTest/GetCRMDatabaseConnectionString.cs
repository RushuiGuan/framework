using Albatross.Config;
using Albatross.Config.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.UnitTest
{
    public class GetCRMDatabaseConnectionString : GetConfig<string>
    {
		public GetCRMDatabaseConnectionString(IGetConfigValue getConfigValue) : base(getConfigValue) { }

		protected override string Key => "connectionString.test";
	}
}

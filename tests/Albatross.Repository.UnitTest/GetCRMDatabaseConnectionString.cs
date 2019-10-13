using Albatross.Config;
using Microsoft.Extensions.Configuration;

namespace Albatross.Repository.UnitTest {
	public class GetCRMDatabaseConnectionString : GetConfig<string>
    {
		public GetCRMDatabaseConnectionString(IConfiguration configuration) : base(configuration) { }

		protected override string Key => "connectionString.test";
	}
}

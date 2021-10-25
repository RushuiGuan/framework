using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Config.UnitTest {
	public class DbConfig : Albatross.Config.Core.IConfigSetting {
		[Required]
		public string DbConnection { get; set; } = null!;

		public void Init(IConfiguration configuration) {
			this.DbConnection = configuration.GetConnectionString("test");
		}

		public void Validate() {
		}
	}
	public class GetDbConfig : GetConfig<DbConfig> {
		public GetDbConfig(IConfiguration configuration) : base(configuration) {
		}

		protected override string Key => "db-config";
	}
}

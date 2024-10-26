using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Config.UnitTest {
	public class DbConfig : ConfigBase {
		public override string Key => "db-config";
		public DbConfig(IConfiguration configuration) : base(configuration) {
		}

		public string? Data { get; set; }
	}
}
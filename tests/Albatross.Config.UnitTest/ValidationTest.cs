using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Config.UnitTest {
	public class ValidationTest : ConfigBase {
		public override string Key => "validation-test1";
		public ValidationTest(IConfiguration configuration) : base(configuration) {
		}
		[Required]
		public string Name { get; } = null!;

		[Required]
		public string Data { get; }=null!;
	}
}
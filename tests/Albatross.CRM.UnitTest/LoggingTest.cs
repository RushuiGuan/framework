using Albatross.CRM.Dto;
using Albatross.CRM.Model;
using Albatross.CRM.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Repository.UnitTest {
	[Collection(DatabaseTestHostCollection.Name)]
	public class LoggingTest {
		private readonly DatabaseTestHost host;

		public LoggingTest(DatabaseTestHost host) {
			this.host = host;
		}

		[Fact]
		public void Run() {
			var logger = host.Provider.GetService<ILogger<LoggingTest>>();
			logger.LogInformation("test 123");
		}
	}
}

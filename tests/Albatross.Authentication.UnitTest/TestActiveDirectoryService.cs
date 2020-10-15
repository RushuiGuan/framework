using Albatross.Authentication.Core;
using Albatross.Authentication.Server;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Authentication.UnitTest {
	public class TestActiveDirectoryService: IClassFixture<MyTestHost>
	{
		private readonly MyTestHost host;

		public TestActiveDirectoryService(MyTestHost host) {
			this.host = host;
		}

		[Fact]
		public async Task TestSelf() {
			using var scope = host.Create();

			var svc = scope.Get<ActiveDirectoryUserProfileService>();
			var current = scope.Get<IGetCurrentUser>().Get();
			var user = await svc.GetUser(current);
			Assert.NotNull(user);
		}
	}
}

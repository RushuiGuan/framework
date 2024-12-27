using System;
using Xunit;

namespace Albatross.Authentication.UnitTest {
	public class TestWindowsAuthentication {
		[Fact]
		public void Run() {
			var user = new Windows.GetCurrentWindowsUser().Get();
			Assert.Equal(Environment.UserName, user);
		}
	}
}
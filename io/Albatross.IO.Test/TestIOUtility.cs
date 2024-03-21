using Xunit;
using Albatross.IO;

namespace Albatross.Test.IO {
	public class TestIOUtility {

		[Fact]
		public void TestEnsureDirectory() {
			@"c:\temp\test\me\like".EnsureDirectory();
		}
	}
}

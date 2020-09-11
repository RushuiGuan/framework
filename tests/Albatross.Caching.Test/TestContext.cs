using Polly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestContext {

		[Fact]
		public void TestEmptyOperationKey() {
			Context c = new Context();
			Assert.True(string.IsNullOrEmpty(c.OperationKey));
			Assert.Null(c.OperationKey);
		}
	}
}

using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Albatross.Caching.Test {
	public class TestCacheKeyRemoval {
		[Fact]
		public void TestRemoveSelfAndChildren() {
			var mock = new Mock<ICacheKeyManagement>();
			mock.Setup(x => x.FindKeys(It.IsAny<string>()))
				.Returns(new string[] { "test:1:1:1", "test:2:1" });
			var keyMgmt = mock.Object;
			var keys = new ICacheKey[] {
				new CacheKey("test", "1"),
				new CacheKey("test", "1"),
				new CacheKey("test", "2")
			};
			keyMgmt.RemoveSelfAndChildren(keys);
			mock.Verify(x => x.FindKeys("test:1:*"), Times.Once);
			mock.Verify(x => x.FindKeys("test:2:*"), Times.Once);
			mock.Verify(x => x.Remove(new string[] { "test:1:1:1", "test:2:1" }), Times.Once);
			mock.VerifyNoOtherCalls();
		}

		[Fact]
		public void TestResetCach() {
			var mock = new Mock<ICacheKeyManagement>();
			mock.Setup(x => x.FindKeys(It.IsAny<string>()))
				.Returns(new string[] { "test:1:1:1", "test:2:1" });
			var keyMgmt = mock.Object;
			var keys = new ICacheKey[] {
				new CacheKey("test", "1"),
				new CacheKey("test", "1"),
				new CacheKey("test", "2"),
				new CacheKey("xx", "2")
			};
			keyMgmt.Reset(keys);
			mock.Verify(x => x.FindKeys("test:*"), Times.Once);
			mock.Verify(x => x.FindKeys("xx:*"), Times.Once);
			mock.Verify(x => x.Remove(new string[] { "test:1:1:1", "test:2:1" }), Times.Once);
			mock.VerifyNoOtherCalls();
		}
	}
}

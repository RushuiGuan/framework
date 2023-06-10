using Xunit;

namespace Albatross.Repository.Test {
	public class EntityTest {
		[Fact]
		public void TestMapping() {
			var entity = new ImmutableEntity();
			Assert.Throws<System.ComponentModel.DataAnnotations.ValidationException>(
				() => entity.ValidateByDataAnnotations()
			);
		}
	}
}

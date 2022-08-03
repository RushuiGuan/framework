using Albatross.Repository.Core;
using System;
using Xunit;

namespace Albatross.Repository.Test {
	public class EntityTest {
		[Fact]
		public void TestMapping() {
			var entity = new ImmutableEntity(String.Empty);
			Assert.Throws<System.ComponentModel.DataAnnotations.ValidationException>(
				() => entity.ValidateByDataAnnotations()
			);
		}
	}
}

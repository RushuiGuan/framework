using Albatross.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Albatross.Test.Validation {

	public class Parent2 {
		[CollectionValidation()]
		public IEnumerable<Child> Children { get; set; }
	}

	public class TestCollectionValidation {
		[Fact]
		public void TestFailure() {
			var parent = new Parent2 { };
			Validator.ValidateObject(parent, new ValidationContext(parent));

			parent.Children = new List<Child>();
			Validator.ValidateObject(parent, new ValidationContext(parent));

			parent.Children = new Child[] { 
				new Child{ }
			};
			Assert.Throws<ValidationException>(() => Validator.ValidateObject(parent, new ValidationContext(parent), true));
		}

		[Fact]
		public void TestTimespan() {
			DateTime? d1 = DateTime.UtcNow;
			DateTime? d2 = null;
			double? result = (d2 - d1)?.TotalMilliseconds;
			Assert.Null(result);

		}
	}
}

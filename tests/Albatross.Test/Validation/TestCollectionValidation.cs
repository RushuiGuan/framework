using Albatross.Validation;
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
	}
}

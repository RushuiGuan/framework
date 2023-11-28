using Albatross.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Albatross.Test.Validation {

	public class Parent3 {
		[RequiredCollection()]
		public IList<Child> Children { get; set; }
	}

	public class TestRequiredCollectionValidation {
		[Fact]
		public void TestFailure() {
			var parent = new Parent3 { };
			Assert.Throws<ValidationException>(() => Validator.ValidateObject(parent, new ValidationContext(parent), true));

			parent.Children = new List<Child>();
			Assert.Throws<ValidationException>(() => Validator.ValidateObject(parent, new ValidationContext(parent), true));

			parent.Children = new Child[] { 
				new Child{ }
			};
			Validator.ValidateObject(parent, new ValidationContext(parent));
		}
	}
}

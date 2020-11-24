using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Albatross.Validation {
	/// <summary>
	/// Make sure that a collection is not empty.  The property type has to be ICollection
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class RequiredCollectionAttribute : ValidationAttribute {
		protected override ValidationResult IsValid(object value, ValidationContext context) {
			if (value == null) {
				return new ValidationResult($"{context.ObjectType.Name}.{context.MemberName} is required");
			} else {
				var items = value as ICollection;
				if (items == null) {
					return new ValidationResult($"{context.ObjectType.Name}.{context.MemberName} should be an ICollection");
				}
				if(items.Count > 0) { 
					return ValidationResult.Success;
				}
				return new ValidationResult($"{context.ObjectType.Name}.{context.MemberName} should have at least one item");
			}
		}

		public override bool IsValid(object value) => (value as ICollection)?.Count > 0;
	}
}
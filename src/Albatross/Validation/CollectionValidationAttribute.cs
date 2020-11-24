using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Albatross.Validation {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field| AttributeTargets.Parameter, AllowMultiple =false)]
	public class CollectionValidationAttribute : ValidationAttribute {
		protected override ValidationResult IsValid(object value, ValidationContext context) {
			if (value == null) {
				return ValidationResult.Success;
			} else {
				var items = value as ICollection;
				if(items == null) {
					return new ValidationResult($"{context.ObjectType.Name}.{context.MemberName} should be an ICollection");
				}
				List<ValidationResult> results = new List<ValidationResult>();
				foreach (var item in items) {
					if (!Validator.TryValidateObject(item, new ValidationContext(item), results)) {
						return new ValidationResult($"{context.ObjectType.Name}.{context.MemberName}: {results.First().ErrorMessage}");
					}
				}
				return ValidationResult.Success;
			}
		}

		public override bool IsValid(object value) {
			if (value == null) {
				return true;
			} else {
				var items = value as ICollection;
				if (items == null) { return false; }
				List<ValidationResult> results = new List<ValidationResult>();
				foreach (var item in items) {
					if (!Validator.TryValidateObject(item, new ValidationContext(item), results)) {
						return false;
					}
				}
				return true;
			}
		}
	}
}
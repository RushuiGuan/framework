using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Albatross.Validation {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field| AttributeTargets.Parameter, AllowMultiple =false)]
	public class RecursiveValidationAttribute : ValidationAttribute {
		protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
			if (value == null) {
				return ValidationResult.Success;
			} else {
				List<ValidationResult> results = new List<ValidationResult>();
				if (Validator.TryValidateObject(value, new ValidationContext(value), results)) {
					return ValidationResult.Success;
				} else {
					return new ValidationResult($"{validationContext.MemberName}: {results.First().ErrorMessage}");
				}
			}
		}
		public override bool IsValid(object value) {
			if (value == null) {
				return true;
			} else {
				List<ValidationResult> results = new List<ValidationResult>();
				return Validator.TryValidateObject(value, new ValidationContext(value), results);
			}
		}
	}
}

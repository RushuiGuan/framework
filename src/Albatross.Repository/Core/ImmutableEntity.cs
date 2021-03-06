﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class ImmutableEntity {
		public const int UserNameLength = 128;
		public DateTime CreatedUtc { get; protected set; }

		[Required]
		[MaxLength(UserNameLength)]
		public string CreatedBy { get; protected set; }
		
		protected void Create(string user) {
			this.CreatedBy = user;
			this.CreatedUtc = DateTime.UtcNow;
			this.Validate();
		}

		public virtual void Validate() {
			Validator.ValidateObject(this, new ValidationContext(this), true);
		}
	}
}

using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class ImmutableEntity<UserType> {
		public DateTime CreatedUTC { get; protected set; }
		public UserType CreatedBy { get; protected set; }
		protected ImmutableEntity() { }
		protected ImmutableEntity(UserType user) : this() {
			Create(user);
		}
		public void Create(UserType user) {
			this.CreatedBy = user;
			this.CreatedUTC = DateTime.UtcNow;
			this.Validate();
		}
		public virtual void Validate() {
			Validator.ValidateObject(this, new ValidationContext(this), true);
		}
	}

	public class ImmutableEntityMap<T, UserType> : EntityMap<T> where T: ImmutableEntity<UserType> {
		public virtual string TableName => this.GetType().Name;
		public override void Map(EntityTypeBuilder<T> builder) {
			builder.ToTable(TableName);
			builder.Property(p => p.CreatedBy).IsRequired();
			builder.Property(p => p.CreatedUTC).IsRequired();
		}
	}
}

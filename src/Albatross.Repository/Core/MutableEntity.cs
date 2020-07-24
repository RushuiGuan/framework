using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace Albatross.Repository.Core {
	public class MutableEntity<UserType> {
		public DateTime CreatedUTC { get; protected set; }
		public DateTime ModifiedUTC { get; protected set; }

		public UserType CreatedBy { get; protected set; }
		public UserType ModifiedBy { get; protected set; }

		protected MutableEntity() { }
		protected MutableEntity(UserType user, IDbSession session) {
			CreatedUTC = DateTime.UtcNow;
			CreatedBy = user;
			this.CreateOrUpdate(user, session);
		}
		public void CreateOrUpdate(UserType user, IDbSession session) {
			if (session.IsChanged(this)) {
				ModifiedUTC = DateTime.UtcNow;
				ModifiedBy = user;
			}
			this.Validate();
		}

		public virtual void Validate() {
			Validator.ValidateObject(this, new ValidationContext(this), true);
		}
	}


	public class MutableEntityMap<T, UserType> : EntityMap<T> where T:MutableEntity<UserType> {
		public virtual string TableName => typeof(T).Name;
		public override void Map(EntityTypeBuilder<T> builder) {
			builder.ToTable(TableName);

			builder.Property(p => p.CreatedBy).IsRequired();
			builder.Property(p => p.CreatedUTC).IsRequired();
			builder.Property(p => p.ModifiedBy).IsRequired();
			builder.Property(p => p.ModifiedUTC).IsRequired();
		}
	}
}

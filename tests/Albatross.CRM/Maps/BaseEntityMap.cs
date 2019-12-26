using Albatross.CRM.Model;
using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Maps {
	public class BaseEntityMap<T> : EntityMap<T> where T : BaseEntity<User> {
		public override void Map(EntityTypeBuilder<T> builder) {
			builder.OwnsOne<User>(args => args.CreatedBy);
			builder.OwnsOne<User>(args => args.ModifiedBy);
			
			builder.Property(p => p.Created).IsRequired();
			builder.Property(p => p.Modified).IsRequired();
		}
	}
}

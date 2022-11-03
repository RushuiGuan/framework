﻿using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.ByEFCore {
	public class ImmutableEntityMap<T> : EntityMap<T> where T: ImmutableEntity {
		public override void Map(EntityTypeBuilder<T> builder) {
			base.Map(builder);
			builder.HasKey(x => x.Id);
			builder.Property(p => p.CreatedBy).IsRequired();
			builder.Property(p => p.CreatedUtc).IsRequired().UtcDateTimeProperty();
		}
	}
}

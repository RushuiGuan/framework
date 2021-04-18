using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Albatross.Repository.ByEFCore {
	public class MutableEntityMap<T> : EntityMap<T> where T:MutableEntity {
		public override void Map(EntityTypeBuilder<T> builder) {
			base.Map(builder);
			builder.Property(p => p.CreatedBy)
				.IsRequired();

			builder.Property(p => p.CreatedUtc)
				.IsRequired()
				.HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

			builder.Property(p => p.ModifiedBy)
				.IsRequired();

			builder.Property(p => p.ModifiedUtc)
				.IsRequired()
				.HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); 
		}
	}
}

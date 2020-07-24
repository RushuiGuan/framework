using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Albatross.Repository.ByEFCore {
	public class ImmutableEntityMap<T> : EntityMap<T> where T: ImmutableEntity {
		public virtual string TableName => typeof(T).Name;
		public override void Map(EntityTypeBuilder<T> builder) {
			builder.ToTable(TableName);
			builder.Property(p => p.CreatedBy)
				.IsRequired()
				.HasMaxLength(ImmutableEntity.UserNameLength);

			builder.Property(p => p.CreatedUTC)
				.IsRequired()
				.HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
		}
	}
}

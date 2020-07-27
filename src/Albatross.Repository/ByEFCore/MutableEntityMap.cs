using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Albatross.Repository.ByEFCore {
	public class MutableEntityMap<T> : EntityMap<T> where T:MutableEntity {
		public virtual string TableName => typeof(T).Name;
		public override void Map(EntityTypeBuilder<T> builder) {
			builder.ToTable(TableName);

			builder.Property(p => p.CreatedBy)
				.IsRequired();

			builder.Property(p => p.CreatedUTC)
				.IsRequired()
				.HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

			builder.Property(p => p.ModifiedBy)
				.IsRequired();

			builder.Property(p => p.ModifiedUTC)
				.IsRequired()
				.HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc)); 
		}
	}
}

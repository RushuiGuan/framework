using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.ByEFCore {
	public class ImmutableEntityMap<T, UserType> : EntityMap<T> where T: ImmutableEntity<UserType> {
		public virtual string TableName => typeof(T).Name;
		public override void Map(EntityTypeBuilder<T> builder) {
			builder.ToTable(TableName);
			builder.Property(p => p.CreatedBy).IsRequired();
			builder.Property(p => p.CreatedUTC).IsRequired();
		}
	}
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.SqlServer {
	public class ImmutableEntityMap<T> : EntityMap<T> where T: ImmutableEntity {
		public override void Map(EntityTypeBuilder<T> builder) {
			base.Map(builder);
			builder.Property(p => p.CreatedBy).IsRequired();
			builder.Property(p => p.CreatedUtc).IsRequired().UtcDateTimeProperty();
		}
	}
}

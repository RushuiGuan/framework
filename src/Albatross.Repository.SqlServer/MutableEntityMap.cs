using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.SqlServer {
	public class MutableEntityMap<T> : EntityMap<T> where T:MutableEntity {
		public override void Map(EntityTypeBuilder<T> builder) {
			base.Map(builder);

			builder.Property(p => p.CreatedBy).IsRequired();
			builder.Property(p => p.ModifiedBy).IsRequired();
			
			builder.Property(p => p.CreatedUtc).IsRequired().UtcDateTimeProperty();
			builder.Property(p => p.ModifiedUtc).IsRequired().UtcDateTimeProperty();
		}
	}
}

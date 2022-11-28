using Albatross.Repository.Core;
using Albatross.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.ByEFCore {
	public class DateLevelEntityEntityMap<T> : EntityMap<T> where T : DateLevelEntity {
		public override string TableName => typeof(T).Name;

		public override void Map(EntityTypeBuilder<T> entityTypeBuilder) {
			base.Map(entityTypeBuilder);
			entityTypeBuilder.HasKey(p => new { p.Id, p.StartDate }).IsClustered();
			entityTypeBuilder.Property(p => p.CreatedUtc).UtcDateTimeProperty();
			entityTypeBuilder.Property(p => p.ModifiedUtc).UtcDateTimeProperty();
		}
	}
}
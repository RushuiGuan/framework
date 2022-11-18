using Albatross.Repository.Core;
using Albatross.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.ByEFCore {
	public class DateLevelEntityEntityMap<T> : EntityMap<DateLevelEntity<T>> where T : class {
		private readonly DateLevelEntityOwnedNavBuilder<T> ownedNavBuilder;
		public override string TableName => typeof(T).Name;

		public DateLevelEntityEntityMap(DateLevelEntityOwnedNavBuilder<T>? ownedNavBuilder = null) {
			this.ownedNavBuilder = ownedNavBuilder ?? new DateLevelEntityOwnedNavBuilder<T>();
		}

		public override void Map(EntityTypeBuilder<DateLevelEntity<T>> entityTypeBuilder) {
			base.Map(entityTypeBuilder);
			entityTypeBuilder.HasKey(p => new { p.Id, p.StartDate }).IsClustered();
			entityTypeBuilder.Property(p => p.CreatedUtc).UtcDateTimeProperty();
			entityTypeBuilder.OwnsOne(p => p.Entity, b => this.ownedNavBuilder.Build(b));
		}
	}
	public class DateLevelEntityOwnedNavBuilder<T> where T : class {
		public virtual void Build(OwnedNavigationBuilder<DateLevelEntity<T>, T> builder) {
			foreach (var property in typeof(T).GetProperties()) {
				builder.Property(property.PropertyType, property.Name).HasColumnName(property.Name);
			}
		}
	}
}
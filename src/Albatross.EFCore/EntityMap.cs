using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.EFCore {
	public abstract class EntityMap<T> : IBuildEntityModel, IEntityMap<T> where T : class {
		public virtual string TableName => typeof(T).Name;
		public virtual string HiLo => $"{typeof(T).Name}HiLo";

		protected virtual void BuildTable(TableBuilder builder) { }
		public virtual void Map(EntityTypeBuilder<T> builder) {
			builder.ToTable<T>(TableName, this.BuildTable);
		}
		public void Build(ModelBuilder builder) {
			var entityTypeBuilder = builder.Entity<T>();
			Map(entityTypeBuilder);
		}
	}
}
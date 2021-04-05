using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.ByEFCore {

	public abstract class EntityMap<T> : IBuildEntityModel, IEntityMap<T> where T : class {
		public virtual string TableName => typeof(T).Name;
		public virtual void Map(EntityTypeBuilder<T> builder) {
			builder.ToTable(TableName);
		}
		public void Build(ModelBuilder builder) {
			var entityTypeBuilder = builder.Entity<T>();
			Map(entityTypeBuilder);
		}
	}
}

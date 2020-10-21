using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.ByEFCore {

	public abstract class EntityMap<T> : IBuildEntityModel, IEntityMap<T> where T : class {
		public abstract void Map(EntityTypeBuilder<T> builder);
		public void Build(ModelBuilder builder) {
			var entityTypeBuilder = builder.Entity<T>();
			Map(entityTypeBuilder);
		}
	}
}

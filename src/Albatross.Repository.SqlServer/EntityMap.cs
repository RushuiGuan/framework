using System;
using System.Collections.Generic;
using System.Text;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.SqlServer {

	public abstract class EntityMap<T> : IBuildEntityModel, IEntityMap<T> where T : class {
		public virtual string TableName => typeof(T).Name;
		public virtual bool Temporal => false;
		public virtual string HiLo => $"{typeof(T).Name}HiLo";

		public virtual void Map(EntityTypeBuilder<T> builder) {
			builder.ToTable(TableName, b => b.IsTemporal(this.Temporal));
		}
		public void Build(ModelBuilder builder) {
			var entityTypeBuilder = builder.Entity<T>();
			Map(entityTypeBuilder);
		}
	}
}

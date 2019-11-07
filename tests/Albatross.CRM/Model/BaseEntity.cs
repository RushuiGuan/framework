using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Model {
	public class BaseEntity {
		protected BaseEntity() { }
		public BaseEntity(int user) {
			CreatedBy = user;
			Created = DateTime.UtcNow;
		}

		public void Update(int user) {
			ModifiedBy = user;
			Modified = DateTime.UtcNow;
		}
		public int CreatedBy { get; set; }
		public int ModifiedBy { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public DateTimeOffset CreatedUTC => new DateTimeOffset(Created, TimeSpan.Zero);
		public DateTimeOffset ModifiedUTC => new DateTimeOffset(Modified, TimeSpan.Zero);
	}

	public class BaseEntityMap<T> : Albatross.Repository.ByEFCore.EntityMap<T> where T:BaseEntity {
		public override void Map(EntityTypeBuilder<T> builder) {
			builder.Property(a => a.Created);
			builder.Property(a => a.CreatedBy);
			builder.Property(a => a.Modified);
			builder.Property(a => a.ModifiedBy);
		}
	}
}

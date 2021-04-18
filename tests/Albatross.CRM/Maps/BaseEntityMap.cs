using Albatross.CRM.Model;
using Albatross.Repository.ByEFCore;
using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Maps {
	public class BaseEntityMap<T> : EntityMap<T> where T : MutableEntity {
		public override void Map(EntityTypeBuilder<T> builder) {
			builder.OwnsOne<string>(args => args.CreatedBy);
			builder.OwnsOne<string>(args => args.ModifiedBy);
			
			builder.Property(p => p.CreatedUtc).IsRequired();
			builder.Property(p => p.ModifiedUtc).IsRequired();
		}
	}
}

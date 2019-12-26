using Albatross.CRM.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CRM.Maps {
	public class FooMap : BaseEntityMap<Foo> {
		public override void Map(EntityTypeBuilder<Foo> builder) {
			base.Map(builder);
			builder.HasKey(p => p.ID);
			builder.Property(p => p.Name);
		}
	}
}

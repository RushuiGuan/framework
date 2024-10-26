using Albatross.EFCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sample.EFCore.Models.Test {
	public class EntityInADiffNameSpace {
		public int Id { get; set; }
	}
	public class EntityInADiffNameSpaceEntityMap : EntityMap<EntityInADiffNameSpace> {
		public override void Map(EntityTypeBuilder<EntityInADiffNameSpace> builder) {
			base.Map(builder);
			builder.HasKey(i => i.Id);
		}
	}
}
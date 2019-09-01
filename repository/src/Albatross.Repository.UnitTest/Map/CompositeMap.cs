using Albatross.Repository.UnitTest.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository.UnitTest.Map {
	public class CompositeMap : Albatross.Repository.ByEFCore.EntityMap<Composite> {
		public override void Map(EntityTypeBuilder<Albatross.Repository.UnitTest.Model.Composite> builder) {
			builder.ToTable(nameof(Composite), Constant.Schema);
            builder.HasKey(nameof(Composite.App), nameof(Composite.Name));

            builder.Property(args => args.App);
            builder.Property(args => args.Name);
            builder.Property(args => args.Value);
        }
	}
}

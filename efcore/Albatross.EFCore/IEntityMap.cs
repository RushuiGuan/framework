using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.EFCore {
	public interface IEntityMap<T> where T : class {
		void Map(EntityTypeBuilder<T> builder);
	}
}
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Albatross.Repository {
	public interface IEntityMap<T> where T : class {
		void Map(EntityTypeBuilder<T> builder);
	}
}

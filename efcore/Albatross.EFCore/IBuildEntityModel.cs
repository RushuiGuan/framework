using Microsoft.EntityFrameworkCore;

namespace Albatross.EFCore {
	public interface IBuildEntityModel {
		void Build(ModelBuilder builder);
	}

	public interface IBuildEntityModel<T> : IBuildEntityModel where T : IDbSession {
	}
}
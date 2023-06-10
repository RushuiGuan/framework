using Microsoft.EntityFrameworkCore;

namespace Albatross.Repository {
	public interface IBuildEntityModel {
		void Build(ModelBuilder builder);
	}

	public interface IBuildEntityModel<T> : IBuildEntityModel where T : IDbSession {
	}
}

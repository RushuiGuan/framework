using Microsoft.EntityFrameworkCore;

namespace Albatross.Repository.ByEFCore {
	public interface IBuildEntityModel {
		void Build(ModelBuilder builder);
	}

	public interface IBuildEntityModel<T> : IBuildEntityModel where T : Core.IDbSession {
	}
}

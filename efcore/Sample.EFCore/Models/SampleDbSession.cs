using Albatross.EFCore;
using Albatross.EFCore.AutoCacheEviction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Sample.EFCore.Models {
	public interface ISampleDbSession : IDbSession { }

	public class SampleDbSession : DbSession, ISampleDbSession {
		public SampleDbSession(AutoCacheEvictionDbSessionEventHander autoCacheEviction, DbContextOptions<SampleDbSession> option, ILogger<SampleDbSession>? logger) : base(option, logger) { 
			this.SessionEventHandlers.Add(autoCacheEviction);
		}
		public SampleDbSession(DbContextOptions<SampleDbSession> option) : base(option, null) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasDefaultSchema(My.Schema.Sample);
		}
	}
}

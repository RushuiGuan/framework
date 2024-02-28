using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Sample.EFCore.Models {
	public interface ISampleDbSession : IDbSession { }

	public class SampleDbSession : DbSession, ISampleDbSession {
		public SampleDbSession(DbContextOptions<SampleDbSession> option, ILogger<SampleDbSession>? logger) : base(option, logger) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasDefaultSchema(My.Schema.Sample);
		}
	}
}

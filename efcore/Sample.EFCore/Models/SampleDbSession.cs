using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Sample.EFCore.Models {
	public interface ISampleDbSession : IDbSession { }

	public class SampleDbSession : DbSessionWithEventHandlers, ISampleDbSession {
		public SampleDbSession(DbContextOptions<SampleDbSession> option, IDbEventSessionProvider eventSessionProvider) 
			: base(option, eventSessionProvider) { 
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasDefaultSchema(My.Schema.Sample);
		}
	}
}

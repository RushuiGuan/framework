using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Sample.EFCore.Models {
	public interface ISampleDbSession : IDbSession { }

	public class SampleDbSession : DbSessionWithEventHandlers, ISampleDbSession {
		public SampleDbSession(DbContextOptions option, IDbEventSessionProvider eventSessionProvider)
			: base(option, eventSessionProvider) {
		}
		protected SampleDbSession(DbContextOptions option) : base(option) { }
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.BuildEntityModels();
			modelBuilder.HasDefaultSchema(My.Schema.Sample);
		}
	}
}
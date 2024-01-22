using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;

namespace Sample.EFCore.Models {
	public interface ISampleDbSession : IDbSession { }

	public class SampleDbSession : DbSession , ISampleDbSession {
		public SampleDbSession(DbContextOptions<SampleDbSession> option) : base(option) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasDefaultSchema(My.Schema.Sample);
		}
	}
}

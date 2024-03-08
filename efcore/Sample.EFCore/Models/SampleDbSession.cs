using Albatross.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Sample.EFCore.Models {
	public interface ISampleDbSession : IDbSession { }

	public class SampleDbSession : DbSession, ISampleDbSession {
		public SampleDbSession(DbContextOptions<SampleDbSession> option, IDbChangeEventHandlerFactory changeEventHandlerFactory, ILogger<SampleDbSession>? logger) 
			: base(option, changeEventHandlerFactory, logger) { 
		}
		public override bool UseChangeEventHandler => true;

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasDefaultSchema(My.Schema.Sample);
		}
	}
}

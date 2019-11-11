using System.Reflection;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;

namespace Albatross.CRM.Repository {
	public class CRMDbSession : DbSession {
		public CRMDbSession(DbContextOptions<CRMDbSession> option) : base(option) { }
		public override Assembly[] EntityModelAssemblies => new Assembly[] { typeof(CRMDbSession).Assembly };

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasSequence(CRMConstant.Hilo, CRMConstant.Schema).StartsAt(1).IncrementsBy(1);
		}
	}
}
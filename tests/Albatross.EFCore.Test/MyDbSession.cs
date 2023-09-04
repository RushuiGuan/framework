using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.EFCore.Test {
	public interface IMyDbSession : IDbSession { }
	public class MyDbSession : DbSession, IMyDbSession {
		public MyDbSession(DbContextOptions<MyDbSession> option) : base(option) {
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.HasDefaultSchema(Constant.Schema);
		}
	}
}

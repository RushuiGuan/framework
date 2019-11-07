using System.Reflection;
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;

namespace Albatross.CRM.Repository {
	public class CRMDbSession : DbSession {
		public CRMDbSession(DbContextOptions<CRMDbSession> option) : base(option) { }
		public override Assembly[] EntityModelAssemblies => new Assembly[] { typeof(CRMDbSession).Assembly };
	}
}
using Albatross.Repository.ByEFCore;
using Microsoft.EntityFrameworkCore;

namespace Albatross.Repository.UnitTest {
	public class CRMDbSession : DbSession {
		public CRMDbSession(DbContextOptions<CRMDbSession> option) : base(option) { }
	}
}
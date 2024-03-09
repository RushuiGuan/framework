using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Albatross.EFCore.Test {
	public class TestSessionEventHandler : IDbSessionEventHandler {
		public TestSessionEventHandler(GetCurrentTestUser getCurrentTestUser) {
			this.getCurrentTestUser = getCurrentTestUser;
		}
		public void OnAddedEntry(EntityEntry entry) { }
		public void OnDeletedEntry(EntityEntry entry) { }
		public void OnModifiedEntry(EntityEntry entry) { }

		static int i = 0;
		public static void ResetCounter() {
			i = 0;
		}
		private readonly GetCurrentTestUser getCurrentTestUser;

		public void PreSave(IDbSession session) {
			getCurrentTestUser.User = $"user{i++}";
		}
		public Task PostSave() => Task.CompletedTask;
	}
}

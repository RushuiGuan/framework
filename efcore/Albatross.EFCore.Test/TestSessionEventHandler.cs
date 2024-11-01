using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Albatross.EFCore.Test {
	public class TestSessionEventHandler : IDbSessionEventHandler {
		public TestSessionEventHandler(GetCurrentTestUser getCurrentTestUser) {
			this.getCurrentTestUser = getCurrentTestUser;
		}
		public override string ToString() => nameof(TestSessionEventHandler);

		public void OnAddedEntry(EntityEntry entry) { }
		public void OnDeletedEntry(EntityEntry entry) { }
		public void OnModifiedEntry(EntityEntry entry) { }

		int i = 0;
		private readonly GetCurrentTestUser getCurrentTestUser;

		public Task PreSave(IDbSession session) {
			getCurrentTestUser.User = $"user{i++}";
			return Task.CompletedTask;
		}
		public Task PostSave() => Task.CompletedTask;
	}
}
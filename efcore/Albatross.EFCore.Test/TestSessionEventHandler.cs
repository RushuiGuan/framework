using Albatross.Hosting.Test;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Albatross.EFCore.Test {
	public class TestSessionEventHandler : IDbSessionEventHandler {
		public TestSessionEventHandler(GetCurrentTestUser getCurrentTestUser) {
			this.getCurrentTestUser = getCurrentTestUser;
		}
		public override string ToString() => nameof(TestSessionEventHandler);

		public Task OnAddedEntry(EntityEntry entry) => Task.CompletedTask;
		public Task OnDeletedEntry(EntityEntry entry) => Task.CompletedTask;
		public Task OnModifiedEntry(EntityEntry entry) => Task.CompletedTask;

		int i = 0;
		private readonly GetCurrentTestUser getCurrentTestUser;

		public Task PreSave(IDbSession session) {
			getCurrentTestUser.User = $"user{i++}";
			return Task.CompletedTask;
		}
		public Task PostSave() => Task.CompletedTask;
	}
}

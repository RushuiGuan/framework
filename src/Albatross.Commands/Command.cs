using System;
using System.Threading.Tasks;

namespace Albatross.Commands {
	public abstract class Command {
		public Command(string id) {
			this.Id = id;
		}
		public string Id { get; init; }
		public abstract Type ReturnType { get; }
		public abstract void SetException(Exception err);
		public abstract void SetResult(object obj);

		public DateTime? SubmittedDateTimeUtc { get; private set; }
		public DateTime? StartedDateTimeUtc { get; private set; }
		public DateTime EndDateTimeUtc { get; protected set; }
		public bool Success { get; protected set; }

		public void MarkStart() => this.StartedDateTimeUtc = DateTime.UtcNow;
		public void MarkSubmitted() => this.SubmittedDateTimeUtc = DateTime.UtcNow;

		public CommandDto CreateDto() {
			return new CommandDto(Id, ReturnType) {
				SubmitedDateTimeUtc = SubmittedDateTimeUtc,
				StartDateTimeUtc = StartedDateTimeUtc,
				WaitTime = (StartedDateTimeUtc - SubmittedDateTimeUtc)?.TotalMilliseconds,
				RunTime = (EndDateTimeUtc - StartedDateTimeUtc)?.TotalMilliseconds,
				Success = Success,
			};
		}
	}

	public abstract class Command<K> : Command where K : notnull {
		public Command(string id) : base(id) {
		}

		public override Type ReturnType => typeof(K);
		public Task<K> Task => taskCompletionSource.Task;
		TaskCompletionSource<K> taskCompletionSource = new TaskCompletionSource<K>();

		public override void SetException(Exception err) {
			taskCompletionSource.SetException(err);
			EndDateTimeUtc = DateTime.UtcNow;
			Success = false;
		}
		public override void SetResult(object obj) {
			if (obj is K) {
				this.taskCompletionSource.SetResult((K)obj);
				EndDateTimeUtc = DateTime.UtcNow;
				Success = true;
			} else {
				this.SetException(new ArgumentException($"Command {Id} cannot set result with incorrect type.  expected: {typeof(K).Name}, received: {obj?.GetType()?.Name}"));
			}
		}
	}
}
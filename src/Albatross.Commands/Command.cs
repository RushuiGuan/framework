﻿using System;
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
		public bool? Success { get; protected set; }

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
		protected internal Task<K> Task => taskCompletionSource.Task;
		TaskCompletionSource<K> taskCompletionSource = new TaskCompletionSource<K>();

		public override void SetException(Exception err) {
			EndDateTimeUtc = DateTime.UtcNow;
			Success = false;
			
			// make sure this is the last step in this function
			// SetException and SetResult is also a blocking call.  Will causes deadlocks if not kicked off using a different thread
			System.Threading.Tasks.Task.Run(() => taskCompletionSource.SetException(err));
		}
		public override void SetResult(object obj) {
			if (obj is K) {
				EndDateTimeUtc = DateTime.UtcNow;
				Success = true;
				// make sure this is the last step in this function
				// SetException and SetResult is also a blocking call.  Will causes deadlocks if not kicked off using a different thread
				System.Threading.Tasks.Task.Run(() => taskCompletionSource.SetResult((K)obj));
			} else {
				this.SetException(new ArgumentException($"Command {Id} cannot set result with incorrect type.  expected: {typeof(K).Name}, received: {obj?.GetType()?.Name}"));
			}
		}
	}
}
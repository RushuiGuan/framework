using System;
using System.Threading.Tasks;

namespace Albatross.Messaging.Eventing {
	public interface ISubscriberCallback {
		DateTime? Acked { get; set; }
		Task<Subscription> Task { get; }
		void SetResult();
		void SetException(Exception error);
		ISubscriber Subscriber { get; }
	}

	public class SubscriberCallback : ISubscriberCallback {
		public ulong Id { get; init; }
		public Task<Subscription> Task => taskCompletionSource.Task;
		public DateTime? Acked { get; set; }

		public ISubscriber Subscriber => this.subscription.Subscriber;
		private TaskCompletionSource<Subscription> taskCompletionSource = new TaskCompletionSource<Subscription>();
		private readonly Subscription subscription;

		public SubscriberCallback(ulong id, Subscription subscription) {
			Id = id;
			this.subscription = subscription;
		}

		public void SetException(Exception err) {
			// make sure this is the last step in this function
			// SetException and SetResult is also a blocking call.  Will causes deadlocks if not kicked off using a different thread
			System.Threading.Tasks.Task.Run(() => taskCompletionSource.SetException(err));
		}
		public void SetResult() => System.Threading.Tasks.Task.Run(() => this.taskCompletionSource.SetResult(this.subscription));
	}
}
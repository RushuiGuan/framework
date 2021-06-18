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
	}

	public abstract class Command<K> :Command{
		public Command(string id) : base(id) { }

		public override Type ReturnType => typeof(K);
		public Task<K> Task => taskCompletionSource.Task;
		TaskCompletionSource<K> taskCompletionSource = new TaskCompletionSource<K>();
		public void SetResult(K k) => taskCompletionSource.SetResult(k);
		public override void SetException(Exception err) => taskCompletionSource.SetException(err);
		public override void SetResult(object obj) {
			if(obj is K) {
				this.SetResult((K)obj);
			} else {
				throw new ArgumentException();
			}
		}
	}
}

using Albatross.Collections;
using Microsoft.Extensions.Logging;
using NetMQ;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.Messaging.ReqRep {
	public class WorkerRegistry : IEnumerable<Worker> {
		public const string DefaultService = "default";
		private readonly Dictionary<string, Worker> registry = new Dictionary<string, Worker>();
		private readonly Dictionary<string, Queue<Worker>> queues = new Dictionary<string, Queue<Worker>>();
		private readonly ILogger<WorkerRegistry> logger;

		public WorkerRegistry(ILogger<WorkerRegistry> logger) {
			this.logger = logger;
		}

		public Worker Add(string identity, IEnumerable<string> services) {
			bool newWorker = false;
			var worker = registry.GetOrAdd(identity, () => {
				logger.LogInformation("new worker: {name}", identity);
				newWorker = true;
				return new Worker(identity);
			});

			if (newWorker) {
				RegisterNewServices(services, worker);
			} else {
				var current = GetCurrentServices(worker);
				current.Merge(services, args => args, args => args, null,
					src => {
						RegisterNewService(src, worker);
					},
					dst => {
						RemoveService(dst, worker);
					}
				);
			}
			return worker;
		}


		private IEnumerable<string> GetCurrentServices(Worker worker) {
			List<string> list = new List<string>();
			foreach (var item in queues) {
				if (item.Value.Contains(worker)) {
					list.Add(item.Key);
				}
			}
			return list;
		}
		private void RegisterNewService(string service, Worker worker) {
			var queue = queues.GetOrAdd(service, () => new Queue<Worker>());
			queue.Enqueue(worker);
		}
		private void RegisterNewServices(IEnumerable<string> services, Worker worker) {
			foreach (var service in services) {
				RegisterNewService(service, worker);
			}
		}
		private void RemoveService(string service, Worker worker) {
			if (queues.TryGetValue(service, out var queue)) {
				if (queue.Count == 1) {
					queues.Remove(service);
				} else {
					var newQueue = new Queue<Worker>();
					while (queue.TryDequeue(out var item)) {
						if (item != worker) {
							newQueue.Enqueue(item);
						}
					}
					queues[service] = newQueue;
				}
			}
		}

		public bool TryFindNextAvailableWorker(string? service, [NotNullWhen(true)] out Worker? worker) {
			if (string.IsNullOrEmpty(service)) {
				service = DefaultService;
			}
			if (queues.TryGetValue(service, out var queue)) {
				while (queue.Count > 0) {
					worker = queue.Dequeue();
					if (worker.IsActive) {
						queue.Enqueue(worker);
						return true;
					}
				}
				queues.Remove(service);
			}
			worker = null;
			return false;
		}
		public bool TryGetWorker(string identity, [NotNullWhen(true)] out Worker? worker) => registry.TryGetValue(identity, out worker);
		public IEnumerator<Worker> GetEnumerator() => registry.Values.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}

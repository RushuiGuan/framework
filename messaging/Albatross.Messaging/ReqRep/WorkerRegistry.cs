using Albatross.Collections;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.Messaging.ReqRep {
	public class WorkerRegistry {
		public const string DefaultService = "default";
		/// <summary>
		/// worker dictionary with its identity as the key
		/// </summary>
		private readonly Dictionary<string, Worker> workerDict = new Dictionary<string, Worker>();
		/// <summary>
		/// a dictionary with the service name as the key and worker queue as the value
		/// </summary>
		private readonly Dictionary<string, Queue<Worker>> serviceDict = new Dictionary<string, Queue<Worker>>();
		private readonly ILogger<WorkerRegistry> logger;

		public WorkerRegistry(ILogger<WorkerRegistry> logger) {
			this.logger = logger;
		}

		public Worker Add(string identity, IEnumerable<string> services) {
			bool newWorker = false;
			var worker = workerDict.GetOrAdd(identity, () => {
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
			foreach (var item in serviceDict) {
				if (item.Value.Contains(worker)) {
					list.Add(item.Key);
				}
			}
			return list;
		}
		private void RegisterNewService(string service, Worker worker) {
			var queue = serviceDict.GetOrAdd(service, () => new Queue<Worker>());
			queue.Enqueue(worker);
		}
		private void RegisterNewServices(IEnumerable<string> services, Worker worker) {
			foreach (var service in services) {
				RegisterNewService(service, worker);
			}
		}
		private void RemoveService(string service, Worker worker) {
			if (serviceDict.TryGetValue(service, out var queue)) {
				if (queue.Count == 1) {
					serviceDict.Remove(service);
				} else {
					var newQueue = new Queue<Worker>();
					while (queue.TryDequeue(out var item)) {
						if (item != worker) {
							newQueue.Enqueue(item);
						}
					}
					serviceDict[service] = newQueue;
				}
			}
		}

		public bool TryFindNextAvailableWorker(string? service, [NotNullWhen(true)] out Worker? worker) {
			if (string.IsNullOrEmpty(service)) {
				service = DefaultService;
			}
			if (serviceDict.TryGetValue(service, out var queue)) {
				// go through the worker queue and look for an active worker
				// if found put it in the end of the queue and return
				// if none is found, remove the queue from the service dict
				while (queue.Count > 0) {
					var item = queue.Dequeue();
					if (item.State == WorkerState.Connected) {
						queue.Enqueue(item);
						worker = item;
						return true;
					}
				}
				// code will only reach here if no worker is active
				serviceDict.Remove(service);
			}
			worker = null;
			return false;
		}
		public bool TryGetWorker(string identity, [NotNullWhen(true)] out Worker? worker) => workerDict.TryGetValue(identity, out worker);
	}
}
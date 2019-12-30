using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.Repository.Core {
	public interface IRepository<T> {
		IDbSession DbSession { get; }
		IQueryable<T> Items { get; }
		IEnumerable<T> Local { get; }
		void Add(T t);
		void AddRange(IEnumerable<T> items);

		void Update(T obj);
		void UpdateRange(IEnumerable<T> items);

		void Remove(T obj);
		void RemoveRange(IEnumerable<T> items);
        T GetItem(params object[] keys);
	}
}

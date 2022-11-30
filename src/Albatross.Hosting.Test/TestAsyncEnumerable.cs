using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;



namespace Albatross.Hosting.Test {
	public class TestAsyncEnumerableQuery<T> : IQueryable<T>, IAsyncQueryProvider, IAsyncEnumerable<T> {
		EnumerableQuery<T> value;

		public TestAsyncEnumerableQuery(IEnumerable<T> enumerable) {
			this.value = new EnumerableQuery<T>(enumerable);
		}

		public TestAsyncEnumerableQuery(Expression expression) {
			this.value = new EnumerableQuery<T>(expression);
		}

		public Type ElementType => typeof(T);
		public Expression Expression => value.AsQueryable().Expression;
		public IQueryProvider Provider => this;

		public object? Execute(Expression expression) => ((IQueryProvider)value).Execute(expression);
		public TResult Execute<TResult>(Expression expression) => ((IQueryProvider)value).Execute<TResult>(expression);

		// here the TElement is Task<IEnumerable<DataType>>
		// the result would be IEnumerable<DataType>
		// we have to cast result back to TElement, which the compiler has no knowledge of type
		public TElement ExecuteAsync<TElement>(Expression expression, CancellationToken cancellationToken = default) {
			var result = this.AsQueryable().Provider.Execute(expression);
			if (result != null) {
				Type resultType = result.GetType();
				Type taskType = typeof(Task<>).MakeGenericType(resultType);
				if (typeof(TElement).IsAssignableFrom(taskType)) {
					var method = typeof(Task).GetMethod(nameof(Task.FromResult), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
					if (method != null) {
						var methodInfo = method.MakeGenericMethod(resultType);
						var obj = methodInfo.Invoke(null, new object?[] { result });
						return (TElement)obj!;
					}
					return default!;
				} else {
					throw new ArgumentException(expression.ToString());
				}
			}
			return default!;
		}
		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) => this.GetAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
		public IEnumerator GetEnumerator() => value.AsQueryable().GetEnumerator();

		IQueryable IQueryProvider.CreateQuery(Expression expression) {
			return ((IQueryProvider)this.value).CreateQuery(expression);
		}

		IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression) {
			return new TestAsyncEnumerableQuery<TElement>(expression);
		}

		async IAsyncEnumerable<T> GetAsyncEnumerable() {
			foreach (var item in value) {
				yield return await Task.FromResult(item);
			}
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator() {
			return this.value.AsEnumerable<T>().GetEnumerator();
		}
	}
}

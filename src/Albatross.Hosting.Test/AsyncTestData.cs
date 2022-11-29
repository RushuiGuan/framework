using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Albatross.Hosting.Test {
	public class AsyncTestData<T> : IQueryable<T>, IAsyncEnumerable<T> where T : class {
		IQueryable<T> items;
		
		public Type ElementType => items.ElementType;
		public Expression Expression => items.Expression;
		public IQueryProvider Provider { get; init; }
		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) {
			return this.GetAsyncEnumerable().GetAsyncEnumerator(cancellationToken);
		}
		async IAsyncEnumerable<T> GetAsyncEnumerable(){
			foreach (var item in this) {
				yield return await Task.FromResult(item);
			}
		}

		public IEnumerator<T> GetEnumerator() => this.items.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

		public AsyncTestData(IEnumerable<T> items) {
			this.items = items.AsQueryable();
			Mock<IAsyncQueryProvider> asyncProviderMock = new Mock<IAsyncQueryProvider>();
			asyncProviderMock.Setup(args => args.ExecuteAsync<Task<IQueryable<T>>>(It.IsAny<Expression>(), CancellationToken.None))
				.Returns(Task.FromResult<IQueryable<T>>(this));
			asyncProviderMock.Setup(args => args.ExecuteAsync<Task<T>>(It.IsAny<Expression>(), CancellationToken.None))
				.Returns(Task.FromResult<T>(this.items.First()));
			asyncProviderMock.Setup(args => args.CreateQuery<T>(It.IsAny<Expression>())).Returns(this);
			this.Provider = asyncProviderMock.Object;
		}
	}
}
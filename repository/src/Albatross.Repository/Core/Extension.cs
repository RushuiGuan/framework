
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Albatross.Reflection;
using Albatross.Repository.ByEFCore;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Albatross.Repository.Core {
	public static class Extension {
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) {
			foreach (var item in items) {
				collection.Add(item);
			}
		}

		public static Task<int> SaveChangesAsync<T>(this IRepository<T> repo, CancellationToken cancellationToken = default) {
			return repo.DbSession.SaveChangesAsync(cancellationToken);
		}


		public static void Merge<Dst, Src, TKey>(this IEnumerable<Dst> dst, IEnumerable<Src> src,
		  Func<Dst, TKey> dstKeySelector, Func<Src, TKey> srcKeySelector,
		  Action<Dst, Src> matched, Action<Src> notMatchedByDst, Action<Dst> notMatchedBySrc) {
			var dstArray = dst.ToArray();
			if(src == null) { src = new Src[0]; }
			Dictionary<TKey, Src> srcDict = new Dictionary<TKey, Src>();
			List<Src> newItems = new List<Src>();

			foreach (var item in src) {
				TKey key = srcKeySelector(item);
				if (object.Equals(key, default(TKey))) {
					newItems.Add(item);
				} else {
					srcDict.Add(key, item);
				}
			}
			foreach (var item in dstArray) {
				TKey key = dstKeySelector(item);
				if (srcDict.TryGetValue(key, out Src srcItem)) {
					matched?.Invoke(item, srcItem);
					srcDict.Remove(key);
				} else {
					notMatchedBySrc?.Invoke(item);
				}
			}
			foreach (var item in srcDict.Values) {
				notMatchedByDst?.Invoke(item);
			}
			foreach (var item in newItems) {
				notMatchedByDst?.Invoke(item);
			}
		}

		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action) {
			foreach (var item in items) {
				action(item);
			}
			return items;
		}

		public static void Execute(this IDbSession dbSession, CommandDefinition command) {
			try {
				dbSession.DbConnection.Execute(command);
			} catch (SqlException err) when (err.TryConvertError(null, out Exception converted)) {
				{
					throw converted;
				}
			}
		}
		public static IEnumerable<T> Query<T>(this IDbSession dbSession, CommandDefinition command) {
			return dbSession.DbConnection.Query<T>(command);
		}
		public static T ExecuteScalar<T>(this IDbSession dbSession, CommandDefinition command) {
			return dbSession.DbConnection.ExecuteScalar<T>(command);
		}
		public static T First<T>(this IDbSession dbSession, CommandDefinition command) {
			return dbSession.DbConnection.QueryFirst<T>(command);
		}
		public static T FirstOrDefault<T>(this IDbSession dbSession, CommandDefinition command) {
			return dbSession.DbConnection.QueryFirstOrDefault<T>(command);
		}
		public static ITransaction BeginTransaction<T>(this IRepository<T> repository) {
			return repository.DbSession.BeginTransaction();
		}

		
	}
}
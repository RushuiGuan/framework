using Albatross.Repository.Core;
using CsvHelper;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Albatross.Hosting.Test {
	public static class Extension {
		public static T CreateAsyncMockSession<T, K>(this IEnumerable<K> items) where T : class, IDbSession where K:class{
			return CreateAsyncMockSession<T>(mock => mock.CreateAsyncDbSet<K>(items));
		}
		public static T CreateAsyncMockSession<T>(Action<Mock<DbContext>> setupContext)where T : class , IDbSession {
			var dbContextMock = new Mock<DbContext>();
			setupContext(dbContextMock);
			var sessionMock = new Mock<T>();
			sessionMock.Setup(args => args.DbContext).Returns(dbContextMock.Object);
			return sessionMock.Object;
		}
		public static void CreateAsyncDbSet<T>(this Mock<DbContext> dbContextMock, IEnumerable<T> items) where T : class {
			dbContextMock.Setup(args => args.Set<T>()).Returns(items.CreateAsyncDbSet<T>().Object);
		}
		public static Mock<DbSet<T>> CreateAsyncDbSet<T>(this IEnumerable<T> items) where T:class {
			var data = new TestAsyncEnumerableQuery<T>(items);
			var mock = new Mock<DbSet<T>>();
			mock.As<IEnumerable<T>>().Setup(args => args.GetEnumerator()).Returns(((IEnumerable<T>)data).GetEnumerator());
			mock.As<IQueryable<T>>().Setup(args => args.Expression).Returns(((IQueryable)data).Expression);
			mock.As<IQueryable<T>>().Setup(args => args.Provider).Returns(((IQueryable)data).Provider);
			mock.As<IAsyncEnumerable<T>>().Setup(args=>args.GetAsyncEnumerator(CancellationToken.None))
				.Returns(()=> data.GetAsyncEnumerator());
			return mock;
		}

		public static string GetSourceCodeDirectory(this string projectPath) {
			return @$"{System.Environment.GetEnvironmentVariable("DevDirectory")}\{projectPath}";
		}

		public static void DumpCsv(this string query, object parameters, string connectionString, string outputFile) {
			using var conn = new SqlConnection(connectionString);
			var items = conn.Query(query, parameters);
			items.DumpCsv(outputFile);
		}

		public static void DumpCsv<T>(this IEnumerable<T> items, string outputFile) {
			using var writer = new StreamWriter(outputFile);
			using var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture, false);
			csvWriter.WriteRecords(items);
		}

		public static IEnumerable<T> ReadCsv<T>(this string resource) {
			using var stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resource) 
				?? throw new ArgumentException($"Assembly embedded resource {resource} was not found");
			using var reader = new StreamReader(stream);
			var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture, false);
			return csvReader.GetRecords<T>();
		}
	}
}
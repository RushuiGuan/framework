using Albatross.Repository.SqlServer;
using Dapper;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System;

namespace Albatross.Repository.SqlServer {
	public static class Extension {
		public static SqlMapper.ICustomQueryParameter AsTableValuedParameter(this IEnumerable<int> values) {
			DataTable table = new DataTable();
			table.Columns.Add("Value", typeof(int));
			foreach (var item in values) {
				table.Rows.Add(item);
			}
			return table.AsTableValuedParameter("dbo.IntArray");
		}

		public static SqlMapper.ICustomQueryParameter AsTableValuedParameter(this IEnumerable<string> values) {
			DataTable table = new DataTable();
			table.Columns.Add("Value", typeof(string));
			foreach (var item in values) {
				table.Rows.Add(item);
			}
			return table.AsTableValuedParameter("dbo.StringArray");
		}

		static JsonSerializerOptions jsonColumnSerializationOptions = new JsonSerializerOptions {
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault,
			WriteIndented = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
		};
		public static ValueConverter<T, string> GetJsonValueConverter<T>(this IBuildEntityModel _, Func<T> getDefault) {
			return new ValueConverter<T, string>(
				args => JsonSerializer.Serialize(args, jsonColumnSerializationOptions),
				args => JsonSerializer.Deserialize<T>(args, jsonColumnSerializationOptions) ?? getDefault()
			);
		}
	}
}
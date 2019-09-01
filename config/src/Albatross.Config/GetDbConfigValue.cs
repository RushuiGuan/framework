using Albatross.Config.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace Albatross.Config {
	public class GetDbConfigValue : IGetConfigValue {
		ProgramSetting setting;

		public GetDbConfigValue(ProgramSetting setting) {
			this.setting = setting;
		}

		public T Get<T>(string key) {
			string text = GetText(key);
			if (string.IsNullOrEmpty(text)) {
				return default(T);
			}
			using (var reader = new JsonTextReader(new StringReader(text))) {
				JObject obj = JObject.Load(reader);
				return obj.ToObject<T>();
			}
		}

		public string GetText(string name) {
			if (string.IsNullOrEmpty(setting.ConfigDatabaseConnection)) {
				throw new ConfigurationException(typeof(ProgramSetting), nameof(ProgramSetting.ConfigDatabaseConnection));
			}
			using (SqlConnection conn = new SqlConnection(setting.ConfigDatabaseConnection)) {
				using (SqlCommand cmd = new SqlCommand("cfg.GetConfig")) {
					cmd.Connection = conn;
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("app", setting.App);
					cmd.Parameters.AddWithValue("name", name);
					conn.Open();
					object obj = cmd.ExecuteScalar();
					if (obj == DBNull.Value) {
						return null;
					} else {
						return (string)obj;
					}
				}
			}
		}
	}
}

using Albatross.Config.Core;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Data.SqlClient;

namespace Albatross.Config.UnitTest {
    [TestFixture]
    public class TestGetDbConfigValue {
        readonly static string CfgName = nameof(TestGetDbConfigValue);
        readonly static string CfgValue = nameof(TestGetDbConfigValue);

        ServiceCollection svc = new ServiceCollection();
        ServiceProvider provider;

        [OneTimeSetUp]
        public void Setup() {
            svc.AddCustomConfig(this.GetType().Assembly, true);
            svc.AddTransient<GetConfigManagementDatabaseConnection>();
            provider = svc.BuildServiceProvider();
        }


        const string sql = @"
merge cfg.ConfigData dst
using (
	values ('config-unittest', 'TestGetDbConfigValue', 'TestGetDbConfigValue')
) as src (app, name, value)
on src.App = dst.App and src.Name = dst.Name
when matched then update set
	Value = src.Value
when not matched then insert (app, name, value)
values (
	src.App,
	src.Name,
	src.Value
);";

        /// <summary>
        /// The connection string in the program setting object should be read only
        /// </summary>
        [Test]
        public void WritePermissionDenied() {
            try {
				var setting = provider.GetRequiredService<ProgramSetting>();
                using (var conn = new SqlConnection(setting.ConfigDatabaseConnection)) {
                    conn.Open();
                    new SqlCommand(sql, conn).ExecuteNonQuery();
                }
            } catch (SqlException err) when (err.Number == 229) {

            }
        }


        [Test]
        public void Run() {
            var connString = provider.GetRequiredService<GetConfigManagementDatabaseConnection>().Get();
            using (var conn = new SqlConnection(connString)) {
                conn.Open();
                new SqlCommand(sql, conn).ExecuteNonQuery();
            }

            var handle = provider.GetRequiredService<GetDbConfigValue>();
            string text = handle.GetText(CfgName);
            Assert.AreEqual(CfgValue, text);
        }
    }
}

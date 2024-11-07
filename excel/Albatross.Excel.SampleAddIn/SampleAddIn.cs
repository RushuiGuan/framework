using Albatross.Authentication.Windows;
using Albatross.Config;
using Albatross.Hosting.Excel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Albatross.Excel.SampleAddIn {
	public class SampleAddIn : HostedExcelAddIn {
		public override void RegisterServices(IConfiguration configuration, IServiceCollection services) {
			base.RegisterServices(configuration, services);
			services.AddConfig<SampleConfig>();
			services.AddExcelRibbon<SampleRibbon>();
			services.AddSingleton<CellFormatDemo>();
			services.AddSingleton<TableWriteDemo>();
			services.AddSingleton<TableReadWriteDemo>();
			services.AddSingleton<InstrumentService>();
			services.AddWindowsPrincipalProvider();
		}

		protected override void Start(IConfiguration configuration, IServiceProvider provider) {
			provider.UseExcelFunctions<InstrumentService>();
			base.Start(configuration, provider);
		}
	}
}
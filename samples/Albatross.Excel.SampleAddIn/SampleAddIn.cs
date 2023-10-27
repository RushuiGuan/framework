﻿using Albatross.Authentication;
using Albatross.Config;
using Albatross.Hosting.Excel;
using ExcelDna.Integration;
using ExcelDna.Registration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Albatross.Excel.SampleAddIn{
	public class SampleAddIn : HostedExcelAddIn {
		public override void RegisterServices(IConfiguration configuration, EnvironmentSetting envSetting, IServiceCollection services) {
			base.RegisterServices(configuration, envSetting, services);
			services.AddConfig<SampleConfig>();
			services.AddExcelRibbon<SampleRibbon>();
			services.AddSingleton<CellFormatDemo>();
			services.AddSingleton<TableWriteDemo>();
			services.AddSingleton<TableReadWriteDemo>();
			services.AddSingleton<InstrumentService>();
			services.AddWindowsPrincipalProvider();
		}
		protected override void Start(IConfiguration configuration, IServiceProvider provider) {
			//ExcelRegistration.GetExcelFunctions().ProcessAsyncRegistrations().RegisterFunctions();
			provider.UseExcelFunctions<InstrumentService>();
			base.Start(configuration, provider);
		}
	}
}

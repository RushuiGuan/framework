using ExcelDna.Integration.CustomUI;
using System.Runtime.InteropServices;
using Albatross.Reflection;
using ExcelDna.Integration;
using Microsoft.Extensions.Logging;
using Albatross.Hosting.Excel;

namespace Albatross.Excel.SampleAddIn {
	[ComVisible(true)]
	public class SampleRibbon : HostedExcelRibbon {
		public override string GetCustomUI(string ribbonId) => this.GetType().GetEmbeddedFile("ribbon.xml");
		private readonly ILogger<SampleRibbon> logger;
		private readonly CellFormatDemo cellFormatDemoService;
		private readonly TableWriteDemo tableDemo;
		private readonly TableReadWriteDemo readWriteDemo;
		private readonly ShowConfigService showConfigSvc;

		public SampleRibbon(ILogger<SampleRibbon> logger, CellFormatDemo cellFormatDemoService, TableWriteDemo tableDemo, TableReadWriteDemo readWriteDemo, ShowConfigService showConfigSvc) : base(logger) {
			this.logger = logger;
			this.cellFormatDemoService = cellFormatDemoService;
			this.tableDemo = tableDemo;
			this.readWriteDemo = readWriteDemo;
			this.showConfigSvc = showConfigSvc;
		}

		public async void Btn_LoadTestData(IRibbonControl _) => await this.tableDemo.WriteToExcel();
		public void Btn_LoadInstrumentData(IRibbonControl _) => this.readWriteDemo.LoadPriceData();
		public void Btn_SavePrice(IRibbonControl _) => this.readWriteDemo.SavePrice();
		public void Btn_NumberFormats(IRibbonControl _) => this.cellFormatDemoService.NumberFormatsDemo();
		public void Btn_Background(IRibbonControl _) => this.cellFormatDemoService.BackgroundDemo();
		public void Btn_FontProperties(IRibbonControl _) => this.cellFormatDemoService.FontPropertiesDemo();
		public void Btn_PrintColor(IRibbonControl _) => this.cellFormatDemoService.PrintColorDemo();

		public string GetEnvironment(IRibbonControl _) => showConfigSvc.Environment;
		public void Btn_GetConfiguration(IRibbonControl _) {
			ExcelAsyncUtil.QueueAsMacro(new ExcelAction(() => this.showConfigSvc.ShowConfig("SampleAddIn")));
		}
		public void Btn_GetVersion(IRibbonControl _) {
			ExcelAsyncUtil.QueueAsMacro(new ExcelAction(() => this.showConfigSvc.ShowVerison("SampleAddIn", this.GetType().Assembly)));
		}
	}
}
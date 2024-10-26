using Albatross.Hosting.Excel;
using Albatross.Reflection;
using ExcelDna.Integration.CustomUI;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace Albatross.Excel.SampleAddIn {
	[ComVisible(true)]
	public class SampleRibbon : HostedExcelRibbon {
		const string ConfigSheetName = "SampleCfg";
		public override string GetCustomUI(string ribbonId) => this.GetType().GetEmbeddedFile("ribbon.xml");
		private readonly ILogger<SampleRibbon> logger;
		private readonly CellFormatDemo cellFormatDemoService;
		private readonly TableWriteDemo tableDemo;
		private readonly TableReadWriteDemo readWriteDemo;
		private readonly HelpService helpService;

		public SampleRibbon(ILogger<SampleRibbon> logger, CellFormatDemo cellFormatDemoService, TableWriteDemo tableDemo, TableReadWriteDemo readWriteDemo, HelpService helpService) : base(logger) {
			this.logger = logger;
			this.cellFormatDemoService = cellFormatDemoService;
			this.tableDemo = tableDemo;
			this.readWriteDemo = readWriteDemo;
			this.helpService = helpService;
		}

		public async void Btn_LoadTestData(IRibbonControl _) => await this.tableDemo.WriteToExcel();
		public void Btn_LoadInstrumentData(IRibbonControl _) => this.readWriteDemo.LoadPriceData();
		public void Btn_SavePrice(IRibbonControl _) => this.readWriteDemo.SavePrice();
		public void Btn_NumberFormats(IRibbonControl _) => this.cellFormatDemoService.NumberFormatsDemo();
		public void Btn_Background(IRibbonControl _) => this.cellFormatDemoService.BackgroundDemo();
		public void Btn_FontProperties(IRibbonControl _) => this.cellFormatDemoService.FontPropertiesDemo();
		public void Btn_PrintColor(IRibbonControl _) => this.cellFormatDemoService.PrintColorDemo();

		public string GetEnvironment(IRibbonControl _) => helpService.GetEnvironment();
		public void Btn_ShowHelp(IRibbonControl _) => helpService.ShowHelp(ConfigSheetName);
	}
}
using interop = Microsoft.Office.Interop.Excel;
using ExcelDna.Integration.CustomUI;
using System.Runtime.InteropServices;
using Albatross.Reflection;
using ExcelDna.Integration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Albatross.Excel.Table;
using Albatross.Hosting.Excel;
using Albatross.Excel.Sample.Models;

namespace Albatross.Excel.Sample {
	[ComVisible(true)]
	public class SampleRibbon : HostedExcelRibbon {
		public override string GetCustomUI(string ribbonId) => this.GetType().GetEmbeddedFile("ribbon.xml");
		private readonly ILogger<SampleRibbon> logger;
		private readonly CellFormatDemo cellFormatDemoService;
		private readonly TableWriteDemo tableDemo;
		private readonly TableReadWriteDemo readWriteDemo;

		public SampleRibbon(ILogger<SampleRibbon> logger, CellFormatDemo cellFormatDemoService, TableWriteDemo tableDemo, TableReadWriteDemo readWriteDemo) : base(logger) {
			this.logger = logger;
			this.cellFormatDemoService = cellFormatDemoService;
			this.tableDemo = tableDemo;
			this.readWriteDemo = readWriteDemo;
		}

		public async void Btn_LoadTestData(IRibbonControl _) => await this.tableDemo.WriteToExcel();
		public void Btn_LoadInstrumentData(IRibbonControl _) => this.readWriteDemo.LoadPriceData();
		public void Btn_SavePrice(IRibbonControl _) => this.readWriteDemo.SavePrice();
		public void Btn_NumberFormats(IRibbonControl _) => this.cellFormatDemoService.NumberFormatsDemo();
		public void Btn_Background(IRibbonControl _) => this.cellFormatDemoService.BackgroundDemo();
		public void Btn_FontProperties(IRibbonControl _) => this.cellFormatDemoService.FontPropertiesDemo();
		public void Btn_PrintColor(IRibbonControl _) => this.cellFormatDemoService.PrintColorDemo();
	}
}
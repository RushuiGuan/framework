using Albatross.CodeGen.Python.Models;
using Albatross.CodeGen.WebClient.Python;
using System;

namespace Albatross.CodeGen.WebClient {
	public class ConvertApiControllerToPythonClass : IConvertObject<Type, Class> {
		public ConvertApiControllerToPythonClass() {
		}

		public Class Convert(Type from) {
			var @class = new WebApiClass("SustainalyticsProxyService", " / api/sustainalytics");
			return @class;
		}

		object IConvertObject<Type>.Convert(Type from) => this.Convert(from);
	}
}

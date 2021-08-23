using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.Templating {
	public interface IRazorTemplateService {
		IActionResult Render<T>(Microsoft.AspNetCore.Mvc.Controller controller, string viewName, T model);
	}
	public class RazorTemplateService : IRazorTemplateService {
		public IActionResult Render<T>(Microsoft.AspNetCore.Mvc.Controller controller,  string viewName, T model) {
			return controller.View(viewName, model);
		}
	}
}

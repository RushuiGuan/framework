using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient {
	public static class My {
		public const string RouteAttributeClassName = "Microsoft.AspNetCore.Mvc.RouteAttribute";
		public const string HttpMethodAttributeClassName = "Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute";
		public const string HttpGetAttributeClassName = "Microsoft.AspNetCore.Mvc.Routing.HttpGetAttribute";
		public const string HttpPostAttributeClassName = "Microsoft.AspNetCore.Mvc.Routing.HttpPostAttribute";
		public const string HttpPatchAttributeClassName = "Microsoft.AspNetCore.Mvc.Routing.HttpPatchAttribute";
		public const string HttpPutAttributeClassName = "Microsoft.AspNetCore.Mvc.Routing.HttpPutAttribute";
		public const string HttpDeleteAttributeClassName = "Microsoft.AspNetCore.Mvc.Routing.HttpDeleteAttribute";

		public readonly static string[] RouteAttributeClasses = [
			HttpGetAttributeClassName,
			HttpPostAttributeClassName,
			HttpPatchAttributeClassName,
			HttpPutAttributeClassName,
			HttpDeleteAttributeClassName,
			RouteAttributeClassName
		];

		public readonly static string[] HttpMethodAttributeClasses = [
			HttpGetAttributeClassName,
			HttpPostAttributeClassName,
			HttpPatchAttributeClassName,
			HttpPutAttributeClassName,
			HttpDeleteAttributeClassName,
		];


		public const string FromQueryAttributeClassName = "Microsoft.AspNetCore.Mvc.FromQueryAttribute";
		public const string FromRouteAttributeClassName = "Microsoft.AspNetCore.Mvc.FromRouteAttribute";
		public const string FromBodyAttributeClassName = "Microsoft.AspNetCore.Mvc.FromBodyAttribute";

		public const char ForwardSlash = '/';

		public const string ActionResultClassName = "Microsoft.AspNetCore.Mvc.ActionResult";
		public const string ActionResultInterfaceName = "Microsoft.AspNetCore.Mvc.IActionResult";
		public const string TaskClassName = "System.Threading.Tasks.Task";
		public const string GenericTaskClassName = "System.Threading.Tasks.Task<>";
		public const string GenericActionResultClassName = "Microsoft.AspNetCore.Mvc.ActionResult<>";
	}
}

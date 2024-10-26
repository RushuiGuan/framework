using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Albatross.CodeGen.WebClient {
	public static class My {
		public const string RouteAttributeClassName = "Microsoft.AspNetCore.Mvc.RouteAttribute";
		public const string HttpMethodAttributeClassName = "Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute";
		public const string HttpGetAttributeClassName = "Microsoft.AspNetCore.Mvc.HttpGetAttribute";
		public const string HttpPostAttributeClassName = "Microsoft.AspNetCore.Mvc.HttpPostAttribute";
		public const string HttpPatchAttributeClassName = "Microsoft.AspNetCore.Mvc.HttpPatchAttribute";
		public const string HttpPutAttributeClassName = "Microsoft.AspNetCore.Mvc.HttpPutAttribute";
		public const string HttpDeleteAttributeClassName = "Microsoft.AspNetCore.Mvc.HttpDeleteAttribute";


		public const string FromQueryAttributeClassName = "Microsoft.AspNetCore.Mvc.FromQueryAttribute";
		public const string FromRouteAttributeClassName = "Microsoft.AspNetCore.Mvc.FromRouteAttribute";
		public const string FromBodyAttributeClassName = "Microsoft.AspNetCore.Mvc.FromBodyAttribute";

		public const string ActionResultClassName = "Microsoft.AspNetCore.Mvc.ActionResult";
		public const string ActionResultInterfaceName = "Microsoft.AspNetCore.Mvc.IActionResult";
		public const string TaskClassName = "System.Threading.Tasks.Task";
		public const string GenericTaskClassName = "System.Threading.Tasks.Task<>";
		public const string GenericActionResultClassName = "Microsoft.AspNetCore.Mvc.ActionResult<>";
		public const string AsyncEnumerableClassName = "System.Collections.Generic.IAsyncEnumerable<>";

		public const string HttpMethodGet = "Get";
		public const string HttpMethodPost = "Post";
		public const string HttpMethodPatch = "Patch";
		public const string HttpMethodPut = "Put";
		public const string HttpMethodDelete = "Delete";

		public const string JsonStringEnumConverter_ClassName = "System.Text.Json.Serialization.JsonStringEnumConverter";
		public const string JsonConverterAttribute_ClassName = "System.Text.Json.Serialization.JsonConverterAttribute";
	}
}
using Templates.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Templates.WebApi.Controllers {
	[Route("api/entity")]
	[ApiController]
	[Authorize]
	public class EntityController : ControllerBase{
	}
}

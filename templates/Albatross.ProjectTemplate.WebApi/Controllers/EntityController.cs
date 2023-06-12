using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Albatross.Templates.WebApi.Controllers {
	[Route("api/entity")]
	[ApiController]
	[Authorize]
	public class EntityController : ControllerBase{
		[HttpGet("exchange/{enfusionId}")]
		public Task<ExchangeDto> GetExchange(int enfusionId) => svc.GetExchange(enfusionId);
	}
}

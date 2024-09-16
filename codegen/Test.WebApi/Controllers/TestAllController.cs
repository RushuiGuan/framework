using Test.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Test.WebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class TestAllController : ControllerBase {
		[HttpGet("{id}")]
		public Task<MyDto> Get([FromRoute] string id, [FromQuery] string name) => Task.FromResult(new MyDto());
	}
}

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Test.Dto.Classes;

namespace Test.WebApi.Controllers {
	[Route("api/interface-abstract-class-test")]
	[ApiController]
	public class InterfaceAndAbstractClassTestController : ControllerBase {
		[HttpPost("interface-as-param")]
		public Task SubmitByInterface(Dto.Classes.ICommand command) => Task.CompletedTask;

		[HttpPost("abstract-class-as-param")]
		public Task SubmitByAbstractClass(AbstractClass command) => Task.CompletedTask;

		[HttpPost("return-interface-async")]
		public Task<ICommand> ReturnInterfaceAsync() => Task.FromResult<ICommand>(new Command1());

		[HttpPost("return-interface")]
		public ICommand ReturnInterface() => new Command1();

		[HttpPost("return-abstract-class-async")]
		public Task<AbstractClass> ReturnAbstractClassAsync() => Task.FromResult<AbstractClass>(new DerivedClass());

		[HttpPost("return-abstract-class")]
		public AbstractClass ReturnAbstractClass() => new DerivedClass();
	}
}
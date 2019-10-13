using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Albatross.IAM.Api {
	public class PrincipalDto { }

	public class NameDto { }
    [Authorize]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase {

        #region return type test
        [HttpGet]
        public IEnumerable<PrincipalDto> GetNormal() {
            return new PrincipalDto[0];
        }

        [HttpGet]
        public Task<IEnumerable<PrincipalDto>> GetNormalAsync() {
            return Task<IEnumerable<PrincipalDto>>.FromResult<IEnumerable<PrincipalDto>>(null);
        }
        [HttpGet]
        public void GetVoid() {
        }
        [HttpGet]
        public Task GetTask() {
            return new Task(() => { });
        }
        [HttpGet]
        public string GetString() {
            return string.Empty;
        }
        [HttpGet]
        public Task<string> GetStringAsync() {
            return Task<string>.FromResult(string.Empty);
        }
        #endregion

        #region parameter and routing test
        [HttpPost("{id}/{name}")]
        public string RouteOnly(int id, string name) {
            return string.Empty;
        }
        [HttpPost()]
        public string QueryStrings(int id, string name) {
            return string.Empty;
        }

        [HttpPost()]
        public string JsonObject([FromBody]NameDto dto) {
            return string.Empty;
        }
        [HttpPost()]
        public string JsonObject([FromBody]NameDto dto1, [FromBody]NameDto dto2) {
            return string.Empty;
        }
        [HttpPost("{groupID}/{userID}")]
        public string MixedRouteQueryStringAndJson(int groupID, int userID, string name, string criteria, [FromBody]NameDto dto) {
            return string.Empty;
        }
        #endregion
    }
}
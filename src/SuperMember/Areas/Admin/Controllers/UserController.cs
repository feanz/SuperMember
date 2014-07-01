using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using SuperMember.Sample.Code.Services;

namespace SuperMember.Sample.Areas.Admin.Controllers
{
    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }


        [Route("users")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsersAsync(string filter = null, int start = 0, int count = 100)
        {
            var result = await _userService.QueryUsersAsync(filter, start, count);

            return Ok(result);
        }
    }
}
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;
using SuperMember.Sample.Areas.Admin.Services;

namespace SuperMember.Sample.Areas.Admin.Controllers
{
    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        private UserService _userService;

        public UserController()
        {
        }

        public UserController(UserService userService)
        {
            UserService = userService;
        }

        public UserService UserService
        {
            get { return _userService ?? HttpContext.Current.GetOwinContext().GetUserManager<UserService>(); }
            private set { _userService = value; }
        }

        [Route("users")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsersAsync(string filter = null, int start = 0, int count = 100)
        {
            var result = await UserService.QueryUsersAsync(filter, start, count);

            return Ok(result);
        }
    }
}
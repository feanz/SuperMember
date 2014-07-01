using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace SuperMember.Sample.Code.Extensions
{
    public static class UserExtentions
    {
        public static string GetDisplayName(this IPrincipal principal)
        {
            var claimsPrincipal = (ClaimsPrincipal)principal;
            var name = claimsPrincipal.Claims.Where(c => c.Type == "Display Name").Select(c => c.Value).SingleOrDefault();
            return name;
        }
    }
}
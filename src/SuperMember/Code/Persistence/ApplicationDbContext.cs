using Microsoft.AspNet.Identity.EntityFramework;
using SuperMember.Sample.Code.Domain;

namespace SuperMember.Sample.Code.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
    }
}
using Microsoft.AspNet.Identity.EntityFramework;
using SuperMember.Sample.Areas.Admin.Domain;

namespace SuperMember.Sample.Areas.Admin.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
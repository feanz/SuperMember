using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SuperMember.Sample.Areas.Admin.Domain;
using SuperMember.Sample.Areas.Admin.Persistence;

namespace SuperMember.Sample.Areas.Admin.Services
{
    public class UserService : UserManager<User>
    {
        public UserService(IUserStore<User> store)
            : base(store)
        {
        }

        public Task<List<User>> QueryUsersAsync(string filter, int start = 1, int count = 10)
        {
            var query = Users;

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(user => user.UserName.Contains(filter));
            }

            var users = query
                .OrderBy(user => user.UserName)
                .Skip(start)
                .Take(count)
                .ToList();

            return Task.FromResult(users);
        }

        public static UserService Create(IdentityFactoryOptions<UserService> options, IOwinContext context) 
        {
            var manager = new UserService(new UserStore<User>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is: {0}"
            });

            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });

            manager.EmailService = new SendGridIdentityEmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}

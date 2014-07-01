using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SimpleInjector;
using SuperMember.Sample.Code.Domain;
using SuperMember.Sample.Code.Interfaces;
using SuperMember.Sample.Code.Services;

namespace SuperMember.Sample.Code
{
    public class UserServiceFactory : IUserServiceFactory
    {
        private readonly Container _container;

        public UserServiceFactory(Container container)
        {
            _container = container;
        }

        public UserService CreateUserService()
        {
            var service = new UserService(_container.GetInstance<IUserStore<User>>(), _container.GetInstance<IEmailIdentityService>(), _container.GetInstance<ISmsIdentityService>());

            // Configure validation logic for usernames
            service.UserValidator = new UserValidator<User>(service)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            service.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            service.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is: {0}"
            });

            service.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is: {0}"
            });


            var dataProtectionProvider = _container.GetInstance<IAppBuilder>().GetDataProtectionProvider();
            if (dataProtectionProvider != null)
            {
                service.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    TokenLifespan = TimeSpan.FromHours(3)
                };
            }

            return service;
        }
    }
}
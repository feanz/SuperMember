using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Provider;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;
using SuperMember.Sample.Code.Domain;
using SuperMember.Sample.Code.Interfaces;
using SuperMember.Sample.Code.Persistence;
using SuperMember.Sample.Code.Services;

namespace SuperMember.Sample.Code
{
    public class IocConfig
    {
        public static Container RegisterService(IAppBuilder app)
        {
            var container = new Container();

            container.RegisterSingle<IUserServiceFactory, UserServiceFactory>();
            
            container.Register<IEmailIdentityService, SendGridIdentityEmailService>();
            container.Register<ISmsIdentityService, SmsService>();
            container.Register<IAppBuilder>(() => app);

            container.RegisterPerWebRequest<ApplicationDbContext>();
            container.RegisterPerWebRequest<IUserStore<User>>(() =>
            {
                var dbContext = container.GetInstance<ApplicationDbContext>();
                return new UserStore<User>(dbContext);
            });

            container.RegisterPerWebRequest<UserService>(() =>
            {
                var factory = container.GetInstance<IUserServiceFactory>();
                return factory.CreateUserService();
            });

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            return container;
        }
    }
}
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;
using SuperMember.Sample.Code;

[assembly: OwinStartupAttribute(typeof(SuperMember.Sample.Startup))]
namespace SuperMember.Sample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AuthConfig.ConfigureAuth(app);
            IocConfig.RegisterService(app);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}

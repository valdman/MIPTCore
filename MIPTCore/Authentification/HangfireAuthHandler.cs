using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Hosting;

namespace MIPTCore.Authentification
{
    public class HangfireAuthHandler : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpcontext = context.GetHttpContext();
            return httpcontext.User.IsInRole("Admin") || !_hostingEnviroment.IsProduction();
        }

        private readonly IHostingEnvironment _hostingEnviroment;

        public HangfireAuthHandler(IHostingEnvironment hostingEnviroment)
        {
            _hostingEnviroment = hostingEnviroment;
        }
    }
}
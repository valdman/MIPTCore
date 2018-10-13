using Hangfire.Annotations;
using Hangfire.Dashboard;
using MIPTCore.Extensions;

namespace MIPTCore.Authentification
{
    public class HangfireAuthHandler : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpcontext = context.GetHttpContext();
            return httpcontext.User.IsInRole("Admin") || httpcontext.Request.IsLocal();
        }
    }
}
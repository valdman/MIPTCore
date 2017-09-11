using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MIPTCore.Authentification.Handlers
{
    public class IsAuthentificatedAuthHandler : AuthorizationHandler<IsAuthentificated>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAuthentificated requirement)
        {
            if(context.User.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier && 
                                            int.TryParse(c.Value, out _)))
            {
                context.Succeed(requirement);
            } 

            return Task.CompletedTask;
        }
    }
}
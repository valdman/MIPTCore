using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UserManagment;

namespace MIPTCore.Authentification.Handlers
{
    public class IsInRoleRoleAuthHandler : AuthorizationHandler<IsInRole>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsInRole requirement)
        {
            UserRole currentRole;
            if(context.User.Claims.Any(c => c.Type == ClaimTypes.Role && 
                                            Enum.TryParse(c.Value, out currentRole) && 
                                            (currentRole == UserRole.Admin || currentRole == requirement.AccountRole)))
            {
                context.Succeed(requirement);
            } 

            return Task.CompletedTask;
        }
    }
}
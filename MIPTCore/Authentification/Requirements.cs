using Microsoft.AspNetCore.Authorization;
using UserManagment;

namespace MIPTCore.Authentification
{
    public class IsInRole : IAuthorizationRequirement
    {
        public UserRole AccountRole {get; private set;}
        public IsInRole(UserRole accountRole)
        {
            AccountRole = accountRole;
        }
    }

    public class IsAuthentificated : IAuthorizationRequirement
    {

    }
}
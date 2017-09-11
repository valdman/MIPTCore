using System;
using System.Linq;
using System.Security.Claims;

namespace MIPTCore.Authentification
{
    public static class AuthorizationExtensions
    {
        public static int GetId(this ClaimsPrincipal identity)
        {
            var id = -1;
            return identity.Claims.Any(c => c.Type == ClaimTypes.NameIdentifier &&
                                             int.TryParse(c.Value, out id))
                ? id
                : throw new ArgumentException("Bullshit in claims");
        }
    }
}
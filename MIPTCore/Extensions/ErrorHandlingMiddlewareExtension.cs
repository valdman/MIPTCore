using System;
using Microsoft.AspNetCore.Builder;
using MIPTCore.Middlewares;

namespace MIPTCore.Extensions
{   
    public static class ErrorHandlingMiddlewareExtension
    {
        public static IApplicationBuilder DomainErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof (app));
            return app.UseMiddleware<ErrorHandlingMiddleware>(Array.Empty<object>());
        }
    }
}